using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Migrations;
using ModernRecrut.MVC.Models;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace ModernRecrut.MVC.Controllers.Tests
{
    public class PostulationControllerTests
    {
        [Fact]
        public async Task Postulation_POST_PostulationValide_Retourne_RedirectToAction()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
           

            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Type", user.Type.ToString()),
                new Claim("Nom", user.Nom),
                new Claim("Prenom", user.Prenom),
            }, "mock"));

            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            user.Type = TypeOccupation.Candidat;

            fichier = user.Id + "_Cv_";

            List<string> fichierList = new List<string>
            {
                fichier
            };

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            mockGestionEmploisServiceProxy.Setup(x => x.Ajouter(offreEmploi));

            mockUserManager.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            mockGestionDocumentsService.Setup(x => x.ObtenirTout(It.IsAny<string>())).ReturnsAsync(fichierList);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);
            postulationController.ControllerContext = new ControllerContext();
            postulationController.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
            //Quand
            var redirectToAction = await postulationController.Postuler(postulation) as RedirectToActionResult;
            //Alors
            redirectToAction.Should().NotBeNull();
            redirectToAction.ActionName.Should().Be("ListePostulations");
            mockGestionPostulationServiceProxy.Verify(x => x.Ajouter(It.IsAny<Postulation>()));
        }

        [Fact]
        public async Task Postulation_POST_PostulationInValide_Retourne_ViewResult()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            

            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Type", user.Type.ToString()),
                new Claim("Nom", user.Nom),
                new Claim("Prenom", user.Prenom),
            }, "mock"));

            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 170000;
            postulation.DateDisponibilite = DateTime.Now.AddYears(1);

            user.Type = TypeOccupation.Candidat;

            fichier = user.Id + "_C_";

            List<string> fichierList = new List<string>
            {
                fichier
            };

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            mockGestionEmploisServiceProxy.Setup(x => x.Ajouter(offreEmploi));

            mockUserManager.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            mockGestionDocumentsService.Setup(x => x.ObtenirTout(It.IsAny<string>())).ReturnsAsync(fichierList);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);
            postulationController.ControllerContext = new ControllerContext();
            postulationController.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
            //Quand

            var viewResult = await postulationController.Postuler(postulation) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            mockGestionPostulationServiceProxy.Verify(x => x.Ajouter(It.IsAny<Postulation>()), Times.Never);
            var postulationResult = viewResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            postulationController.ModelState.Count.Should().Be(3);
            postulationController.ModelState["PretentionSalariale"].Errors.Should().NotBeEmpty();
            postulationController.ModelState["DateDisponibilite"].Errors.Should().NotBeEmpty();
            postulationController.ModelState["cvTrouve"].Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Postulation_POST_AucunDocuments_Retourne_NotFound()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            

            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Type", user.Type.ToString()),
                new Claim("Nom", user.Nom),
                new Claim("Prenom", user.Prenom),
            }, "mock"));

            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 170000;
            postulation.DateDisponibilite = DateTime.Now.AddYears(1);

            user.Type = TypeOccupation.Candidat;

            fichier = user.Id + "_C_";

            List<string> fichierList = new List<string>
            {

            };

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            mockGestionEmploisServiceProxy.Setup(x => x.Ajouter(offreEmploi));

            mockUserManager.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            mockGestionDocumentsService.Setup(x => x.ObtenirTout(It.IsAny<string>()));

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);
            postulationController.ControllerContext = new ControllerContext();
            postulationController.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
            //Quand

            var actionResult = await postulationController.Postuler(postulation);

            //Alors
            actionResult.Should().BeOfType<NotFoundObjectResult>("Object reference not set to an instance of an object");
        }

        [Fact]
        public async Task Postulation_POST_PasAuthentifier_Retourne_NotFound()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
           

            var postulation = fix.Create<Postulation>();

            postulation.PretentionSalariale = 170000;
            postulation.DateDisponibilite = DateTime.Now.AddYears(1);

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand

            var actionResult = await postulationController.Postuler(postulation);

            //Alors
            actionResult.Should().BeOfType<NotFoundObjectResult>("Object reference not set to an instance of an object");
        }

        [Fact]
        public async Task Edit_GET_PostulationValide_Retourne_ViewResult()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
           

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            mockGestionPostulationServiceProxy.Setup(x => x.Obtenir(id)).ReturnsAsync(postulation);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var viewResult = await postulationController.Edit(id) as ViewResult;
            //Alors
            viewResult.Should().NotBeNull();
            var postulationResult = viewResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
        }
        [Fact]
        public async Task Edit_GET_IDNonValide_Retourne_NotFound()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id++;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            mockGestionPostulationServiceProxy.Setup(x => x.Obtenir(id));

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var actionResult = await postulationController.Edit(id);
            //Alors
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundResult>("Object reference not set to an instance of an object");
        }

        [Fact]
        public async Task Edit_GET_IDNonValide_Retourne_BadRequest()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id++;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            var HttpRequestException = new HttpRequestException();
            mockGestionPostulationServiceProxy.Setup(x => x.Obtenir(id)).Throws(HttpRequestException);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var actionResult = await postulationController.Edit(id);
            //Alors
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<BadRequestObjectResult>(HttpRequestException.Message);
        }       

        [Fact]
        public async Task Edit_POST_PostulationValide_Retourne_RedirectToAction()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });           

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();
            
            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var redirectToAction = await postulationController.Edit(id, postulation) as RedirectToActionResult;
            //Alors
            redirectToAction.Should().NotBeNull();
            redirectToAction.ActionName.Should().Be("Index");
            mockGestionPostulationServiceProxy.Verify(x => x.Modifier(It.IsAny<Postulation>()));
        }

        [Fact]
        public async Task Edit_POST_PostulationNonValide_Retourne_ViewResult()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            fix.RepeatCount = 10;

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now.AddMonths(1);

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();            

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var viewResult = await postulationController.Edit(id, postulation) as ViewResult;
            //Alors
            viewResult.Should().NotBeNull();
            mockGestionPostulationServiceProxy.Verify(x => x.Modifier(It.IsAny<Postulation>()), Times.Never);
            var postulationResult = viewResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            postulationController.ModelState.Count.Should().Be(1);
            postulationController.ModelState["DateDisponibilite"].Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Edit_POST_ErreurAppelAPI_Retourne_ViewResult()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            fix.RepeatCount = 10;

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            var DbUpdateConcurrencyException = new DbUpdateConcurrencyException();
            mockGestionPostulationServiceProxy.Setup(x => x.Modifier(It.IsAny<Postulation>())).Throws(DbUpdateConcurrencyException);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);

            //Quand
            var viewResult = await postulationController.Edit(id, postulation) as ViewResult;
            //Alors
            viewResult.Should().NotBeNull();
            mockGestionPostulationServiceProxy.Verify(x => x.Modifier(It.IsAny<Postulation>()));
            var postulationResult = viewResult.Model as Postulation;
            postulationResult.Should().Be(postulation);            
        }



        [Fact]
        public async Task Edit_POST_ErreurHTTP_Retourne_BadRequest()
        {
            //Etant donné
            Fixture fix = new Fixture();
            fix.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            fix.RepeatCount = 10;

            var id = fix.Create<int>();
            var postulation = fix.Create<Postulation>();
            var offreEmploi = fix.Create<OffreEmploi>();
            var fichier = fix.Create<string>();

            var user = fix.Create<ModernRecrutMVCUser>();

            postulation.Id = id;
            postulation.OffreDemploiID = offreEmploi.Id;
            postulation.IdCandidat = user.Id;
            postulation.PretentionSalariale = 40000;
            postulation.DateDisponibilite = DateTime.Now;

            Mock<IGestionPostulationsService> mockGestionPostulationServiceProxy = new Mock<IGestionPostulationsService>();
            Mock<IGestionEmploisService> mockGestionEmploisServiceProxy = new Mock<IGestionEmploisService>();
            Mock<ILogger<PostulationController>> mockLogger = new Mock<ILogger<PostulationController>>();
            Mock<IGestionDocumentsService> mockGestionDocumentsService = new Mock<IGestionDocumentsService>();
            Mock<UserManager<ModernRecrutMVCUser>> mockUserManager = new Mock<UserManager<ModernRecrutMVCUser>>(new Mock<IUserStore<ModernRecrutMVCUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<ModernRecrutMVCUser>>().Object, new IUserValidator<ModernRecrutMVCUser>[0], new IPasswordValidator<ModernRecrutMVCUser>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<ModernRecrutMVCUser>>>().Object);
            Mock<IWebHostEnvironment> mockWebHostEnvironement = new Mock<IWebHostEnvironment>();
            Mock<IDataProtectionProvider> mockDataProtector = new Mock<IDataProtectionProvider>();

            var HttpRequestException = new HttpRequestException();
            mockGestionPostulationServiceProxy.Setup(x => x.Modifier(postulation)).Throws(HttpRequestException);

            mockWebHostEnvironement.Setup(x => x.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            var postulationController = new PostulationController(mockGestionEmploisServiceProxy.Object, mockWebHostEnvironement.Object, mockGestionPostulationServiceProxy.Object, mockUserManager.Object, mockGestionDocumentsService.Object, mockLogger.Object, mockDataProtector.Object);
            
            //Quand
            var actionResult = await postulationController.Edit(id, postulation);
            //Alors
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<BadRequestObjectResult>(HttpRequestException.Message);
        }
    }
}