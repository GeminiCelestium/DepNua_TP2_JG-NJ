using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace Examen1.MVC.Services
{
    public class UtilitaireService : IUtilitaireService
    {
        public int ObtenirTailleListVille(IEnumerable<OffreEmploi> offreEmplois)
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
