

namespace ModernRecrut.Favoris.API.Models
{
    public class OffreFavoris
    {
        public string Id { get; set; }

        public string IpAdresse { get; set; }

        public ICollection<OffreEmploi> Favoris { get; set; }



    }
}
