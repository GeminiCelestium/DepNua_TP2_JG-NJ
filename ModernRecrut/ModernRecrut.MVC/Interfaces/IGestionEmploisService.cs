using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IGestionEmploisService
    {
        Task<List<OffreEmploi>> ObtenirTout();
        Task<OffreEmploi> Obtenir(int id);

        Task<HttpResponseMessage> Ajouter(OffreEmploi vehicule);

        Task Supprimer(int id);

        Task Modifier(OffreEmploi vehicule);
    }
}
