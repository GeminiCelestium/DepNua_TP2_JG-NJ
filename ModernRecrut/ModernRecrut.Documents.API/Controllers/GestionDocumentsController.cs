using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Documents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionDocumentsController : ControllerBase
    {
        private IWebHostEnvironment _env;

        private IGenererNom _genererNom;

        private string DirectoryPath;

        private readonly string storageConnectionString = "PasEncoreCree";

        public GestionDocumentsController(IWebHostEnvironment env, IGenererNom genererNom)
        {
            _env = env;
            _genererNom = genererNom;
            DirectoryPath = Path.Combine(_env.ContentRootPath, "wwwroot\\documents");
        }

        // GET: api/<GestionDocumentsController>
        [HttpGet("{id}")]
        public IEnumerable<string> Get(string id)
        {
            var fichiersAvecChemin = Directory.GetFiles(DirectoryPath, id + "*");
            var fichierSansChemin = new List<string>();
            foreach (string fichier in fichiersAvecChemin)
            {
                fichierSansChemin.Add(Path.GetFileName(fichier));
            }
            return fichierSansChemin;
        }

        // POST api/<GestionDocumentsController>
        [HttpPost]
        public async Task<IActionResult> EnregistrementDocument(Fichier fichierRecu)
        {
            var codeUtilisateur = fichierRecu.Id;

            byte[] bytes = Convert.FromBase64String(fichierRecu.DataFile);

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("images");
                if (!containerClient.Exists())
                {
                    containerClient.Create();
                }

                string nomImage = _genererNom.GenererNomFichier(codeUtilisateur, "Image", fichierRecu.FileName);

                BlobClient blobClient = containerClient.GetBlobClient(nomImage);
              
                await blobClient.UploadAsync(new MemoryStream(bytes), true);

                return Ok("Image enregistrée avec succès dans le compte de stockage.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }


        [HttpGet("any/{id}")]
        public bool AnyTypeDocumentPourUtilisateur(int id)
        {
            return true;
        }
    }
}
