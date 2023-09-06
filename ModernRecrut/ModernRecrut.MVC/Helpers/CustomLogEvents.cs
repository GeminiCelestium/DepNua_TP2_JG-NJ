namespace ModernRecrut.MVC.Helpers
{
    public class CustomLogEvents
    {
        public static Dictionary<int, string> events = new Dictionary<int, string>()
        {
            {100, "Recherche"},
            {101, "Création"},
            {102, "Modification"},
            {103, "Suppréssion"},
            {104, "Consultation"},
            {400, "Erreur"},
            {404, "Page non trouvée"},
            {405, "Méthode non supportée"},
            {500, "Critique"},
            {501, "Null Reference"}
        };

        public const int Recherche = 100;
        public const int Consultation = 104;
        public const int Creation = 101;
        public const int Modication = 102;
        public const int Suppression = 103;

        public const int Erreur = 400;
        public const int NotFound = 404;
        public const int Critique = 500;
        public const int NullReference = 501;
    }
}
