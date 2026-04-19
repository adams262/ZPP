using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PZPP_Grupa5.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PZPP_Grupa5.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IYouTubeService _youtubeService;
        private readonly IGeminiService _geminiService;

        // [[[ Dependency Injection serwisów YouTubeService i GeminiService ]]]
        public MainViewModel(IYouTubeService youtubeService, IGeminiService geminiService)
        {
            _youtubeService = youtubeService;
            _geminiService = geminiService;
        }

        [ObservableProperty]
        private string videoUrl;

        [ObservableProperty]
        private string tekstWynikowy;

        [ObservableProperty]
        private bool chceStreszczenie;

        [ObservableProperty]
        private bool chceWniosek;

        [ObservableProperty]
        private bool chceTimestamps;

        // [[[ Komenda do przetwarzania wideo ]]]
        [RelayCommand]
        private async Task ProcessVideoAsync()
        {
            if (string.IsNullOrWhiteSpace(VideoUrl))
            {
                TekstWynikowy = "Proszę wprowadzić poprawny URL wideo z YouTube.";
                return;
            }

            TekstWynikowy = "Przetwarzanie wideo, proszę czekać...";

            try
            {
                // [[[ Pobieranie danych z YouTube ]]]
                var youtubeDane = await _youtubeService.GetYouTubeAsync(VideoUrl);
                TekstWynikowy = "Pobrano dane. Trwa analiza, proszę czekać...";

                if (!youtubeDane.CzyTylkoAudio && youtubeDane.Tekst.Contains("<style>"))
                {
                    TekstWynikowy = "Błąd: YouTube zablokował pobieranie napisów. Spróbuj innego filmu.";
                    return;
                }

                // [[[ Przetwarzanie danych przez Gemini AI Studio ]]]
                var wynikPrzetworzony = await _geminiService.GetGeminiAsync(youtubeDane, ChceStreszczenie, ChceWniosek, ChceTimestamps);
                TekstWynikowy = wynikPrzetworzony;

            }
            catch (Exception ex)
            {
                TekstWynikowy = $"Wystąpił błąd: {ex.Message}";
            }
        }
    }
}
