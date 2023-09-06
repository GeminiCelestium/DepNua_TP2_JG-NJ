using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public class OffreEmploi
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Date d'affichage")]
        public DateTime DateAffichage { get; set; }
        [Required]
        [DisplayName("Date de fin")]
        public DateTime DateDeFin { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Poste { get; set; }
        [Required]
        public string? Nom { get; set; }
    }
}
