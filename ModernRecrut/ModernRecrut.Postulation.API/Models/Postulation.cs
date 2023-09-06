namespace ModernRecrut.Postulations.API.Models
{
    public class Postulation : BaseEntity
    {
        public string? IdCandidat { get; set; }

        public int? OffreDemploiID { get; set; }

        public decimal PretentionSalariale { get; set; }

        public DateTime DateDisponibilite { get; set; }

        public virtual ICollection<Note>? Notes { get; set; }

    }
}
