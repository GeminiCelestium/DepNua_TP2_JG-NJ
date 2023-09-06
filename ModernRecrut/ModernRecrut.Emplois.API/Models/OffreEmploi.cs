namespace ModernRecrut.Emplois.API.Models
{
    public class OffreEmploi : BaseEntity
    {
        public DateTime DateAffichage { get; set; }

        public DateTime DateDeFin { get; set; }

        public string? Description { get; set; }

        public string Poste { get; set; }

        public string Nom { get; set; }
    }
}
