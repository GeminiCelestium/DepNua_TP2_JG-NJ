using ModernRecrut.Postulations.API.Interfaces;
using ModernRecrut.Postulations.API.Models;

namespace ModernRecrut.Postulations.API.Helpers
{
    public class GenererEvaluation : IGenererEvaluation
    {
        private const string NomEmeteur = "ApplicationPostulation";
        private const string SalaireMinString = "Salaire inférieur à la norme";
        private const string SalaireMaxString = "Salaire supérieur à la norme";
        private const string SalaireNormeString = "Salaire dans la norme";
        private const string SalaireProcheMaisInferieurString = "Salaire proche de la norme mais inférieur à la norme";
        private const string SalaireProcheMaisSuperieurString = "Salaire proche mais supérieur à la norme";
        private const int SalaireMin = 20000;
        private const int SalaireDeuxiemeTranche = 40000;
        private const int SalaireTroisiemeTranche = 80000;
        private const int SalaireMax = 100000;

        public Note generationNoteEvalution(Postulation postulation)
        {
            if (postulation.PretentionSalariale <= SalaireMin)
            {
                return new Note { NoteRh = SalaireMinString, NoteEmeteur = NomEmeteur, Postulation = postulation };
            }
            else if (postulation.PretentionSalariale > SalaireMin && postulation.PretentionSalariale < SalaireDeuxiemeTranche)
            {
                return new Note { Postulation = postulation, NoteRh = SalaireProcheMaisInferieurString, NoteEmeteur = NomEmeteur };
            }
            else if (postulation.PretentionSalariale >= SalaireDeuxiemeTranche && postulation.PretentionSalariale < SalaireTroisiemeTranche)
            {
                return new Note { Postulation = postulation, NoteRh = SalaireNormeString, NoteEmeteur = NomEmeteur };
            }
            else if (postulation.PretentionSalariale >= SalaireTroisiemeTranche && postulation.PretentionSalariale < SalaireMax)
            {
                return new Note { Postulation = postulation, NoteRh = SalaireProcheMaisSuperieurString, NoteEmeteur = NomEmeteur };
            }
            else
            {
                return new Note { Postulation = postulation, NoteRh = SalaireMaxString, NoteEmeteur = NomEmeteur };
            }
        }
    }
}

