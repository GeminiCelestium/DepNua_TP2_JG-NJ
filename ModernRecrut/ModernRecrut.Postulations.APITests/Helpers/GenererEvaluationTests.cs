using ModernRecrut.Postulations.API.Models;
using Xunit;

namespace ModernRecrut.Postulations.API.Helpers.Tests
{
    public class GenererEvaluationTests
    {
        private const string NomEmeteur = "ApplicationPostulation";
        private const string SalaireMinString = "Salaire inférieur à la norme";
        private const string SalaireMaxString = "Salaire supérieur à la norme";
        private const string SalaireNormeString = "Salaire dans la norme";
        private const string SalaireProcheMaisInferieurString = "Salaire proche de la norme mais inférieur à la norme";
        private const string SalaireProcheMaisSuperieurString = "Salaire proche mais supérieur à la norme";
        

        [Fact]
        public void generationNote_pretentionInferieure20000_retourne_NoteSalairemin()
        {
            //Étant donné

            var generationNote = new GenererEvaluation();
            var postulation = new Postulation()
            {
                PretentionSalariale = 19000
            };
            var note = new Note { NoteRh = SalaireMinString, NoteEmeteur = NomEmeteur, Postulation = postulation };

            //Quand

            var resultat = generationNote.generationNoteEvalution(postulation);
            //Alors
            Assert.Equal(note.NoteRh, resultat.NoteRh);
            Assert.Equal(note.NoteEmeteur, resultat.NoteEmeteur);
            Assert.Equal(note.Postulation, resultat.Postulation);
        }

        [Fact]
        public void generationNote_pretentionSuperiere20000etInferiere40000_retourne_SalaireProcheMaisInferieurString()
        {
            //Étant donné

            var generationNote = new GenererEvaluation();
            var postulation = new Postulation()
            {
                PretentionSalariale = 25000
            };
            var note = new Note { NoteRh = SalaireProcheMaisInferieurString, NoteEmeteur = NomEmeteur, Postulation = postulation };

            //Quand

            var resultat = generationNote.generationNoteEvalution(postulation);
            //Alors
            Assert.Equal(note.NoteRh, resultat.NoteRh);
            Assert.Equal(note.NoteEmeteur, resultat.NoteEmeteur);
            Assert.Equal(note.Postulation, resultat.Postulation);
        }

        [Fact]
        public void generationNote_pretentionSuperiere40000etInferiere80000_retourne_SalaireNormeString()
        {
            //Étant donné

            var generationNote = new GenererEvaluation();
            var postulation = new Postulation()
            {
                PretentionSalariale = 60000
            };
            var note = new Note { NoteRh = SalaireNormeString, NoteEmeteur = NomEmeteur, Postulation = postulation };

            //Quand

            var resultat = generationNote.generationNoteEvalution(postulation);
            //Alors
            Assert.Equal(note.NoteRh, resultat.NoteRh);
            Assert.Equal(note.NoteEmeteur, resultat.NoteEmeteur);
            Assert.Equal(note.Postulation, resultat.Postulation);
        }

        [Fact]
        public void generationNote_pretentionSuperiere80000etInferiere100000_retourne_SalaireProcheMaisSuperieurString()
        {
            //Étant donné

            var generationNote = new GenererEvaluation();
            var postulation = new Postulation()
            {
                PretentionSalariale = 90000
            };
            var note = new Note { NoteRh = SalaireProcheMaisSuperieurString, NoteEmeteur = NomEmeteur, Postulation = postulation };

            //Quand

            var resultat = generationNote.generationNoteEvalution(postulation);
            //Alors
            Assert.Equal(note.NoteRh, resultat.NoteRh);
            Assert.Equal(note.NoteEmeteur, resultat.NoteEmeteur);
            Assert.Equal(note.Postulation, resultat.Postulation);
        }

        [Fact]
        public void generationNote_pretentionSuperiere100000_retourne_SalaireMaxString()
        {
            //Étant donné

            var generationNote = new GenererEvaluation();
            var postulation = new Postulation()
            {
                PretentionSalariale = 110000
            };
            var note = new Note { NoteRh = SalaireMaxString, NoteEmeteur = NomEmeteur, Postulation = postulation };

            //Quand

            var resultat = generationNote.generationNoteEvalution(postulation);
            //Alors
            Assert.Equal(note.NoteRh, resultat.NoteRh);
            Assert.Equal(note.NoteEmeteur, resultat.NoteEmeteur);
            Assert.Equal(note.Postulation, resultat.Postulation);
        }
    }
}