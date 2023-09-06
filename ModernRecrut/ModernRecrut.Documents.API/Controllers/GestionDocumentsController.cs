using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Models;

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
            MemoryStream stream = new MemoryStream(bytes);

            IFormFile file = new FormFile(stream, 0, bytes.Length, fichierRecu.Name, fichierRecu.FileName);

            try
            {
                Dictionary<int, string> mesDocuments = new Dictionary<int, string>();

                string nom = _genererNom.GenererNomFichier(codeUtilisateur, fichierRecu.TypeDocument.ToString(), fichierRecu.FileName);

                //Chemin du fichier avec son nouveau nom
                var documentPath = Path.Combine(DirectoryPath, nom);

                var fileStream = new FileStream(documentPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
                stream.Close();
                fileStream.Close();
                return CreatedAtAction(nameof(EnregistrementDocument), mesDocuments);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("any/{id}")]
        public bool AnyTypeDocumentPourUtilisateur(int id)
        {
            return true;
        }
    }
}
