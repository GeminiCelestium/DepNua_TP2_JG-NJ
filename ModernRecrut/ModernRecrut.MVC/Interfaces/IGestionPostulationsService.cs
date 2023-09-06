using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IGestionPostulationsService
    {
        Task<List<Postulation>> ObtenirTout();
        Task<Postulation> Obtenir(int id);

        Task<HttpResponseMessage> Ajouter(Postulation vehicule);

        Task Supprimer(int id);

        Task Modifier(Postulation vehicule);
    }
}
