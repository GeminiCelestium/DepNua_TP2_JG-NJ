using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Controllers
{
    public class FavorisController : Controller
    {

        private readonly IGestionFavorisService _gestionFavorisServiceProxy;
        private readonly IGestionEmploisService _gestionEmploisServiceProxy;
        private readonly ILogger<FavorisController> _logger;

        public FavorisController(IGestionFavorisService GestionFavorisServiceProxy, IGestionEmploisService GestionEmploisServiceProxy, ILogger<FavorisController> logger)
        {
            _gestionFavorisServiceProxy = GestionFavorisServiceProxy;
            _gestionEmploisServiceProxy = GestionEmploisServiceProxy;
            _logger = logger;
        }
        // GET: FavorisController
        public async Task<ActionResult> Index()
        {

            try
            {
                var offreEmplois = new List<OffreEmploi>();
                var listeFavoris = await _gestionFavorisServiceProxy.ObtenirTout();
                foreach (OffreEmploi favoris in listeFavoris)
                {
                    offreEmplois.Add(favoris);
                }
                return View(offreEmplois);
            }
            catch (Exception e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à l'obtention de la liste de favoris- {e.Message}");
                return BadRequest();
            }
        }

        // GET: FavorisController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            try
            {
                var offreEmplois = new List<OffreEmploi>();
                var listeFavoris = await _gestionFavorisServiceProxy.ObtenirTout();
                foreach (OffreEmploi favoris in listeFavoris)
                {
                    offreEmplois.Add(favoris);
                }
                if (offreEmplois.Any(e => e.Id == id))
                {
                    return View(offreEmplois.FirstOrDefault(e => e.Id == id));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à l'obtention des détails du favoris {id} - {e.Message}");
                return BadRequest();
            }
        }

        // GET: FavorisController/Create
        public async Task<ActionResult> Create(int id)
        {
            try
            {
                var offreEmploi = await _gestionEmploisServiceProxy.Obtenir(id);
                if (offreEmploi != null && ModelState.IsValid)
                {
                    return View(offreEmploi);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la création du favoris {id} - {e.Message}");
                return BadRequest();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OffreEmploi offre)
        {
            try
            {
                await _gestionFavorisServiceProxy.Ajouter(offre);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la création du favoris {offre.Id} - {e.Message}");
                return BadRequest();
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var offreEmploi = await _gestionEmploisServiceProxy.Obtenir(id);
                if (offreEmploi != null && ModelState.IsValid)
                {
                    return View(offreEmploi);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la suppression du favoris {id} - {e.Message}");
                return BadRequest();
            }
        }
        // GET: FavorisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(OffreEmploi offreEmploi)
        {
            try
            {
                await _gestionFavorisServiceProxy.Supprimer(offreEmploi.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la suppression du favoris {offreEmploi.Id} - {e.Message}");
                return BadRequest();
            }

        }

    }
}
