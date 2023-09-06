using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IUtilitaireService
    {
        int ObtenirTailleListVille(IEnumerable<OffreEmploi> offreEmplois);
        int ObtenirTailleOffreEmploi(OffreEmploi offreEmploi);
    }
}
