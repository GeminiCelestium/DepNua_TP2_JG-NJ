using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IGestionFavorisService
    {
        Task<List<OffreEmploi>> ObtenirTout();

        Task<HttpResponseMessage> Ajouter(OffreEmploi favoris);

        Task Supprimer(int id);

    }
}
