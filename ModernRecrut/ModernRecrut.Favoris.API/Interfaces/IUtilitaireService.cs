using ModernRecrut.Favoris.API.Models;

namespace ModernRecrut.Favoris.API.Interfaces
{
    public interface IUtilitaireService
    {
        int ObtenirTailleListOffreEmploi(IEnumerable<OffreEmploi> offreEmplois);
        int ObtenirTailleOffreEmploi(OffreEmploi offreEmploi);
    }
}
