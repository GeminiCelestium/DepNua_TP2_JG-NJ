using System.ComponentModel;

namespace ModernRecrut.Favoris.API.Models
{
    public class OffreEmploi
    {
        public int Id { get; set; }
        [DisplayName("Date d'affichage")]
        public DateTime DateAffichage { get; set; }
        [DisplayName("Date de fin")]
        public DateTime DateDeFin { get; set; }

        public string? Description { get; set; }

        public string? Poste { get; set; }

        public string? Nom { get; set; }
    }
}


