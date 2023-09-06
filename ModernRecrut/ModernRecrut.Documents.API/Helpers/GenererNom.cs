using ModernRecrut.Documents.API.Interfaces;

namespace ModernRecrut.Documents.API.Helpers
{
    public class GenererNom : IGenererNom
    {
        public string GenererNomFichier(string codeUtilisateur, string typeDocument, string fileName)
        {
            //On genere un numéro aléatoire
            string numAleatoire = Guid.NewGuid().ToString();

            string extention = Path.GetExtension(fileName);
            // on prepare le nouveau nom du fichier 
            return codeUtilisateur + "_" + typeDocument + "_" + numAleatoire + $"{extention}";
        }
    }
}
