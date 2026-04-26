using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using PZPP_Grupa5.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PZPP_Grupa5.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IYouTubeService _youtubeService;
        private readonly IGeminiService _geminiService;

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

        // Nowa właściwość do obsługi stanu ładowania
        [ObservableProperty]
        private bool isBusy;

        [RelayCommand]
        private async Task ProcessVideoAsync()
        {
            // Jeśli już trwa przetwarzanie, nie pozwól na kolejne kliknięcie
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(VideoUrl))
            {
                TekstWynikowy = "Proszę wprowadzić poprawny URL wideo z YouTube.";
                return;
            }

            try
            {
                // Włączamy animację ładowania
                IsBusy = true;
                TekstWynikowy = "Przetwarzanie wideo, proszę czekać...";

                // [[[ Pobieranie danych z YouTube ]]]
                var youtubeDane = await _youtubeService.GetYouTubeAsync(VideoUrl);

                if (!youtubeDane.CzyTylkoAudio && youtubeDane.Tekst.Contains("<style>"))
                {
                    TekstWynikowy = "Błąd: YouTube zablokował pobieranie napisów. Spróbuj innego filmu.";
                    return;
                }

                TekstWynikowy = "Pobrano dane. Trwa analiza przez AI, proszę czekać...";

                // [[[ Przetwarzanie danych przez Gemini AI Studio ]]]
                var wynikPrzetworzony = await _geminiService.GetGeminiAsync(youtubeDane, ChceStreszczenie, ChceWniosek, ChceTimestamps);

                TekstWynikowy = wynikPrzetworzony;
            }
            catch (Exception ex)
            {
                TekstWynikowy = $"Wystąpił błąd: {ex.Message}";
            }
            finally
            {
                // Wyłączamy animację ładowania niezależnie od wyniku
                IsBusy = false;
            }
        }
    }
}