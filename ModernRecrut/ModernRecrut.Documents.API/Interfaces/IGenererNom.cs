namespace ModernRecrut.Documents.API.Interfaces
{
    public interface IGenererNom
    {
        public string GenererNomFichier(string codeUtilisateur, string typeDocument, string fileName);
    }
}
