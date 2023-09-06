using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ModernRecrut.MVC.Services
{
    public class GestionEmploisServiceProxy : IGestionEmploisService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GestionEmploisServiceProxy> _logger;
        private const string _ApiUrl = "api/OffreEmplois";

        public GestionEmploisServiceProxy(HttpClient httpClient, ILogger<GestionEmploisServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(OffreEmploi offreEmploi)
        {
            StringContent content = new(JsonConvert.SerializeObject(offreEmploi), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Creation, $"Ajout d'un offre d'emploi {content}");
            try
            {
                var reponse = await _httpClient.PostAsync(_ApiUrl, content);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'ajout d'un offre d'emploi : {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        public async Task<List<OffreEmploi>> ObtenirTout()
        {
            _logger.LogInformation(CustomLogEvents.Consultation, $"Consultation des offres d'emploi");
            try
            {
                return await _httpClient.GetFromJsonAsync<List<OffreEmploi>>(_ApiUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la consultation des offres d'emploi : {ex.Message}");
                return new List<OffreEmploi>();
            }

        }

        public async Task<OffreEmploi> Obtenir(int id)
        {
            _logger.LogInformation(CustomLogEvents.Recherche, $"Recherche d'une offre d'emploi");
            try
            {
                return await _httpClient.GetFromJsonAsync<OffreEmploi>(_ApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la recherche d'une offre d'emploi : {ex.Message}");
                return new OffreEmploi();
            }
        }

        public async Task Supprimer(int id)
        {
            _logger.LogInformation(CustomLogEvents.Suppression, $"Suppression d'une offre d'emploi");
            try
            {
                await _httpClient.DeleteAsync(_ApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la suppression d'une offre d'emploi : {ex.Message}");
            }

        }

        public async Task Modifier(OffreEmploi offreEmploi)
        {
            StringContent content = new(JsonConvert.SerializeObject(offreEmploi), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Modication, $"Modification d'une offre d'emploi{content}");
            try
            {
                await _httpClient.PutAsync(_ApiUrl + "/" + offreEmploi.Id, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la modification d'une offre d'emploi : {ex.Message}");
            }

        }
    }
}
