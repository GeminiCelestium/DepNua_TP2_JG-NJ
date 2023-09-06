using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ModernRecrut.MVC.Services
{
    public class GestionDocumentsServiceProxy : IGestionDocumentsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GestionDocumentsServiceProxy> _logger;
        private const string _apiUrl = "api/GestionDocuments";


        public GestionDocumentsServiceProxy(HttpClient httpClient, ILogger<GestionDocumentsServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(Fichier fichier)
        {
            StringContent content = new(JsonConvert.SerializeObject(fichier), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Creation, $"Ajout d'un fichier {fichier.Name}");
            try
            {
                return await _httpClient.PostAsync(_apiUrl, content);

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'ajout d'un document : {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        public async Task<List<string>> ObtenirTout(string id)
        {
            _logger.LogInformation(CustomLogEvents.Recherche, $"Obtention de tous les Documents");
            try
            {
                return await _httpClient.GetFromJsonAsync<List<string>>(_apiUrl + "/" + id);

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'obtention de tous les documents : {ex.Message}");
                return new List<string>();
            }
        }
        public async Task<bool> AnyCV(Fichier fichier)
        {
            StringContent content = new(JsonConvert.SerializeObject(fichier), Encoding.UTF8, "application/json");
            _logger.LogInformation(CustomLogEvents.Recherche, $"Vérification si l'utilisateur a un curriculum vitae");
            try
            {
                return await _httpClient.GetFromJsonAsync<bool>(_apiUrl + "/" + content);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la vérification du curriculum vitae : {ex.Message}");
                return false;
            }
        }


        //public string ObtenirAddresseAPI()
        //{
        //    return _apiUrl;
        //}
    }
}
