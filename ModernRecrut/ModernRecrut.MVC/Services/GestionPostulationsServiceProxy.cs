using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ModernRecrut.MVC.Services
{
    public class GestionPostulationsServiceProxy : IGestionPostulationsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GestionPostulationsServiceProxy> _logger;
        private const string _ApiUrl = "api/Postulations";
        public GestionPostulationsServiceProxy(HttpClient httpClient, ILogger<GestionPostulationsServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<HttpResponseMessage> Ajouter(Postulation Postulation)
        {
            StringContent content = new(JsonConvert.SerializeObject(Postulation), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Creation, $"Ajout d'une postulation {content}");
            try
            {
                var reponse = await _httpClient.PostAsync(_ApiUrl, content);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'ajout d'une postulation : {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        public async Task<List<Postulation>> ObtenirTout()
        {
            _logger.LogInformation(CustomLogEvents.Consultation, $"Consultation des postulations");
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Postulation>>(_ApiUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la consultation des postulations : {ex.Message}");
                return new List<Postulation>();
            }

        }

        public async Task<Postulation> Obtenir(int id)
        {
            _logger.LogInformation(CustomLogEvents.Recherche, $"Recherche d'une postulation");
            try
            {
                return await _httpClient.GetFromJsonAsync<Postulation>(_ApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la recherche d'une postulation : {ex.Message}");
                return new Postulation();
            }
        }

        public async Task Supprimer(int id)
        {
            _logger.LogInformation(CustomLogEvents.Suppression, $"Suppression d'une postulation");
            try
            {
                await _httpClient.DeleteAsync(_ApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la suppression d'une postulation : {ex.Message}");
            }

        }

        public async Task Modifier(Postulation Postulation)
        {
            StringContent content = new(JsonConvert.SerializeObject(Postulation), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Modication, $"Modification d'une postulation{content}");
            try
            {
                await _httpClient.PutAsync(_ApiUrl + "/" + Postulation.Id, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la modification d'une postulation : {ex.Message}");
            }

        }
    }
}
