using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ModernRecrut.MVC.Services
{
    public class GestionFavorisServiceProxy : IGestionFavorisService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GestionFavorisServiceProxy> _logger;
        private const string _apiUrl = "api/GestionFavoris";

        public GestionFavorisServiceProxy(HttpClient httpClient, ILogger<GestionFavorisServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(OffreEmploi favoris)
        {
            StringContent content = new(JsonConvert.SerializeObject(favoris), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Creation, $"Ajout d'un favoris {content}");
            try
            {
                return await _httpClient.PostAsync(_apiUrl, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'ajout d'un favoris : {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        public async Task<List<OffreEmploi>> ObtenirTout()
        {
            _logger.LogInformation(CustomLogEvents.Recherche, $"Obtention de tous les favoris");
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<OffreEmploi>>(_apiUrl);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'obtention de tous les favoris : {ex.Message}");
                return new List<OffreEmploi>();
            }
        }

        //public async Task<Favoris> Obtenir(int id)
        //{
        //    return await _httpClient.GetFromJsonAsync<Favoris>(_apiUrl + id);
        //}

        public async Task Supprimer(int id)
        {
            _logger.LogInformation(CustomLogEvents.Suppression, $"Suppression d'un favoris");
            try
            {
                await _httpClient.DeleteAsync(_apiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la suppression d'un favoris : {ex.Message}");
            }

        }
    }
}
