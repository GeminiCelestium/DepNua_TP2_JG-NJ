using ModernRecrut.MVC.Utilities.Validation;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public enum TypeDocument
    {
        [Display(Name = "Curriculum Vitae")]
        Cv,
        [Display(Name = "Lettre de motivation")]
        LettreDeMotivation,
        [Display(Name = "Diplôme")]
        Diplome

    }
    public class Fichier
    {
        public string? Id { get; set; }
        public string? DataFile { get; set; }
        public string? Name { get; set; }
        public string? FileName { get; set; }
        [DisplayFormat(NullDisplayText = "Choisir un type pour le document")]
        [Required(ErrorMessage = "Le champ est obligatoire")]
        public TypeDocument? TypeDocument { get; set; }
        [Extensions(ErrorMessage = "Les extensions pdf, docx et doc uniquement sont supportées")]
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public IFormFile? DataFormFile { get; set; }
    }
}
