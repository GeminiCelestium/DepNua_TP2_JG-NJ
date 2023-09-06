using ModernRecrut.Favoris.API.Interfaces;
using ModernRecrut.Favoris.API.Models;

namespace ModernRecrut.Favoris.API.Services
{
    public class UtilitaireService : IUtilitaireService
    {
        public int ObtenirTailleListOffreEmploi(IEnumerable<OffreEmploi> offreEmplois)
        {
            int taille = 0;
            foreach (var offreEmploi in offreEmplois)
            {
                taille += ObtenirTailleOffreEmploi(offreEmploi);
            }
            return taille;
        }

        public int ObtenirTailleOffreEmploi(OffreEmploi offreEmploi)
        {

            int taille = offreEmploi.DateAffichage.ToString().Length;
            taille += offreEmploi.DateDeFin.ToString().Length;
            taille += offreEmploi.Poste.Length;
            taille += offreEmploi.Nom.Length;

            if (offreEmploi.Description != null)
            {
                taille += offreEmploi.Description.Length;
            }

            return taille;
        }
    }
}
