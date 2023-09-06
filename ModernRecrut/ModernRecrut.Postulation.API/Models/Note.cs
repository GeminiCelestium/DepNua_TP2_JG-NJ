namespace ModernRecrut.Postulations.API.Models
{
    public class Note : BaseEntity
    {

        public string NoteRh { get; set; }

        public string NoteEmeteur { get; set; }

        //propriété de naviguation 
        public Postulation Postulation { get; set; }

    }
}
