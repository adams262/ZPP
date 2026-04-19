

using PZPP_Grupa5.Models;

namespace PZPP_Grupa5.Services
{
    public interface IGeminiService
    {
        // [[[ Metody serwisu Gemini ]]]
        // Klucz API do Gemini AI Studio:
        // AIzaSyBME2bJ6pcfLWfbOGAcr5Ln8vF-gDttsE8
        Task<string> GetGeminiAsync(YouTubeDependency dane, bool streszczenie, bool wniosek, bool timestamps);
    }
}
