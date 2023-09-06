using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Controllers
{
    public class OffreEmploisController : Controller
    {
        public readonly IGestionEmploisService _gestionEmploisService;
        public readonly ILogger<OffreEmploisController> _logger;

        public OffreEmploisController(IGestionEmploisService gestionEmploisService, ILogger<OffreEmploisController> logger)
        {
            _gestionEmploisService = gestionEmploisService;
            _logger = logger;
        }
        // GET: OffreEmploisController

        public async Task<ActionResult> Index()
        {
            try
            {
                var offreEmplois = await _gestionEmploisService.ObtenirTout();
                return View(offreEmplois);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à l'obtention des offres d'emplois - {e.Message}");
                return BadRequest();
            }
        }

        // GET: OffreEmploisController/Details/5

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var offreEmploi = await _gestionEmploisService.Obtenir(id);

                if (offreEmploi == null)
                {
                    return NotFound();
                }

                return View(offreEmploi);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à l'obtention des détails de l'offre d'emplois {id} - {e.Message}");
                return BadRequest();
            }

        }

        // GET: OffreEmploisController/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: OffreEmploisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OffreEmploi offreEmploi)
        {
            try
            {
                if (offreEmploi.DateAffichage > DateTime.Now)
                    ModelState.AddModelError("DateAffichage", "La date d'affichage doit etre inférieure ou égale a la date du jour ");

                if (offreEmploi.DateDeFin < DateTime.Now)
                    ModelState.AddModelError("DateDeFin", "La date de fin doit etre supérieure ou égale a la date du jour ");

                if (ModelState.IsValid)
                {
                    await _gestionEmploisService.Ajouter(offreEmploi);

                    return RedirectToAction(nameof(Index));

                }

                return View();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la création de l'offre d'emplois {offreEmploi.Id} - {e.Message}");
                return BadRequest();
            }

        }

        // GET: OffreEmploisController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var offreEmploi = await _gestionEmploisService.Obtenir(id);
                if (offreEmploi == null)
                {
                    _logger.LogInformation(CustomLogEvents.NotFound, $"Erreur rencontré à la modification de l'offre d'emplois {offreEmploi.Id}");
                    return NotFound();
                }
                return View(offreEmploi);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la modification de l'offre d'emplois {id} - {e.Message}");
                return BadRequest();
            }
        }

        // POST: OffreEmploisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OffreEmploi offreEmploi)
        {
            try
            {
                if (offreEmploi.DateAffichage > DateTime.Now)
                    ModelState.AddModelError("DateAffichage", "La date d'affichage doit etre inférieure ou égale a la date du jour ");

                if (offreEmploi.DateDeFin < DateTime.Now)
                    ModelState.AddModelError("DateDeFin", "La date de fin doit etre supérieure ou égale a la date du jour ");

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _gestionEmploisService.Modifier(offreEmploi);
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la modification de l'offre d'emplois {offreEmploi.Id} - {e.Message}");
                        if (!OffreEmploiExists(offreEmploi.Id))
                        {
                            _logger.LogError(CustomLogEvents.NotFound, $"Erreur rencontré à la modification de l'offre d'emplois {offreEmploi.Id} - {e.Message}");
                            return NotFound();
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(offreEmploi);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la modification de l'offre d'emplois {id} - {e.Message}");
                return BadRequest();
            }
        }

        // GET: OffreEmploisController/Delete/5

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var offreEmploi = await _gestionEmploisService.Obtenir(id);
                if (offreEmploi != null)
                {
                    return View(offreEmploi);
                }
                _logger.LogInformation(CustomLogEvents.NotFound, $"Erreur rencontré à la suppression de l'offre d'emplois {offreEmploi.Id}");
                return NotFound();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la suppression de l'offre d'emplois {id} - {e.Message}");
                return BadRequest();
            }
        }

        // POST: OffreEmploisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(OffreEmploi offreEmploi)
        {
            try
            {
                await _gestionEmploisService.Supprimer(offreEmploi.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la suppression de l'offre d'emplois {offreEmploi.Id} - {e.Message}");
                return BadRequest();
            }
        }

        private bool OffreEmploiExists(int id)
        {
            try
            {
                return _gestionEmploisService.ObtenirTout().Result.Any(e => e.Id == id);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur rencontré à la suppression de l'offre d'emplois {id} - {e.Message}");
                return false;
            }
        }
    }
}
