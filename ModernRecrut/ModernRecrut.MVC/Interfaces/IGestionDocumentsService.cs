using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IGestionDocumentsService
    {
        Task<HttpResponseMessage> Ajouter(Fichier document);

        Task<List<string>> ObtenirTout(string id);

        Task<bool> AnyCV(Fichier fichier);

        //public string ObtenirAddresseAPI();

    }
}
