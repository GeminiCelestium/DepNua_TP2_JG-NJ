using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.API.Models;
namespace ModernRecrut.Emplois.API.Data
{
    public class ModernRecrutEmploisContext : DbContext
    {
        public ModernRecrutEmploisContext(DbContextOptions<ModernRecrutEmploisContext> options) : base(options)
        {

        }

        public DbSet<OffreEmploi> OffreEmploi { get; set; }
    }
}
