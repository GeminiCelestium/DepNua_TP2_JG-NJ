using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;


namespace ModernRecrut.MVC.Controllers
{
    public class PostulationController : Controller
    {

        private readonly IGestionEmploisService _gestionEmploisServiceProxy;
        private readonly IGestionPostulationsService _gestionPostulationsServiceProxy;
        private readonly IGestionDocumentsService _gestionDocumentsServiceProxy;
        private readonly UserManager<ModernRecrutMVCUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PostulationController> _logger;
       

        public PostulationController(IGestionEmploisService gestionEmploisServiceProxy, IWebHostEnvironment env, IGestionPostulationsService gestionPostulationsServiceProxy, UserManager<ModernRecrutMVCUser> userManager, IGestionDocumentsService gestionDocumentsServiceProxy, ILogger<PostulationController> logger)
        {
            _gestionEmploisServiceProxy = gestionEmploisServiceProxy;
            _gestionPostulationsServiceProxy = gestionPostulationsServiceProxy;
            _userManager = userManager;
            _env = env;
            _gestionDocumentsServiceProxy = gestionDocumentsServiceProxy;
            _logger = logger;

        }


        // GET: PostulationController
        [Authorize]
        public async Task<ActionResult> ListePostulations()
        {
            try
            {
                var postulations = await _gestionPostulationsServiceProxy.ObtenirTout();
                return View(postulations);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la récupération des postulations - {e.Message} ");
                return BadRequest();
            }
        }

        // GET: PostulationController/Details/5
        [Authorize]
        [Authorize]
        public async Task<ActionResult> Notes(int id)
        {
            try
            {
                var postulation = await _gestionPostulationsServiceProxy.Obtenir(id);

                if (postulation == null)
                {
                    return NotFound();
                }

                return View(postulation);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la récupération de la postulation {id} - {e.Message} ");
                return BadRequest();
            }
        }

        // GET: PostulationController/Create
        [Authorize]
        public async Task<ActionResult> Postuler()
        {
            var numeroIdentifiant = await _userManager.FindByNameAsync(User.Identity.Name);

            

            ViewBag.NumeroIdentifiant = numeroIdentifiant.Id;

            return View();
        }

        // POST: PostulationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Postuler(Postulation postulation)
        {            
                ModelState.Remove("IdCandidat");
                try
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var user = await _userManager.FindByNameAsync(User.Identity.Name);
                        
                        try
                        {
                            var listeFichiers = await _gestionDocumentsServiceProxy.ObtenirTout(user.Id);
                            bool cvTrouve = false;

                            foreach (var fichier in listeFichiers)
                            {
                                if (fichier.Contains(user.Id + "_Cv_"))
                                {
                                    cvTrouve = true;
                                    break;
                                }
                            }


                            if (cvTrouve == false)
                            {
                                ModelState.AddModelError("cvTrouve", "Vous devez avoir un CV");
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la récupération des documents de l'utilisateur {user.Id} - {e.Message} ");
                            return NotFound(e.Message);
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    _logger.LogError(CustomLogEvents.Erreur, $"Erreur l'utilisateur n'est pas authentifié");
                    return NotFound(e.Message);
                }

                if (postulation.PretentionSalariale > 150000)
                    ModelState.AddModelError("PretentionSalariale", "Votre prétentation salariale est au-delà de nos limites");

                if (postulation.DateDisponibilite < DateTime.Today || postulation.DateDisponibilite >= DateTime.Today.AddDays(45))
                    ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure au {DateTime.Today} + 45 jours >");

                if (ModelState.IsValid)
                {
                    await _gestionPostulationsServiceProxy.Ajouter(postulation);

                    return RedirectToAction(nameof(ListePostulations));
                }

                return View(postulation);           
        }

        // GET: PostulationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var postulation = await _gestionPostulationsServiceProxy.Obtenir(id);

                if (postulation == null)
                {
                    return NotFound();
                }

                return View(postulation);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors la modification d'une postulation {id} - {e.Message}");
                return BadRequest(e.Message);
            }

        }

        // POST: PostulationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Postulation postulation)
        {
            try
            {
                if (postulation.DateDisponibilite < DateTime.Today.AddDays(-5) || postulation.DateDisponibilite > DateTime.Today.AddDays(5))
                    ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure au {DateTime.Today} + 5 jours >");

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _gestionPostulationsServiceProxy.Modifier(postulation);
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'appel de l'api pour modification de la postulation {postulation.Id} : {e.Message}");
                        return View(postulation);
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(postulation);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors la modification d'une postulation {id} - {e.Message}");
                return BadRequest(e.Message);
            }
        }

        // GET: PostulationController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var offreEmploi = await _gestionPostulationsServiceProxy.Obtenir(id);
                if (offreEmploi != null)
                {
                    return View(offreEmploi);
                }
                return NotFound();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'appel de l'api pour obtention d'une postulation {id} - {e.Message}");
                return BadRequest();
            }
        }

        // POST: PostulationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Postulation postulation)
        {
            try
            {
                if (postulation.DateDisponibilite < DateTime.Today.AddDays(-5) || postulation.DateDisponibilite > DateTime.Today.AddDays(5))
                    ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure au {DateTime.Today} + 5 jours >");
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _gestionPostulationsServiceProxy.Supprimer(postulation.Id);
                    }
                    catch (HttpRequestException e)
                    {
                        _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de l'appel de l'api pour suppression de la postulation {postulation.Id} : {e.Message}");
                        return View(postulation);
                    }

                    return RedirectToAction(nameof(Index));
                }

                return View(postulation);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la suppression de la postulation {postulation.Id} : {e.Message}");
                return View(postulation);
            }
        }

        private bool PostulationExists(int id)
        {
            try
            {
                return _gestionPostulationsServiceProxy.ObtenirTout().Result.Any(e => e.Id == id);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(CustomLogEvents.Erreur, $"Erreur lors de la vérification de l'existence de la postulation {id} : {e.Message}");
                return false;
            }

        }
    }
}
