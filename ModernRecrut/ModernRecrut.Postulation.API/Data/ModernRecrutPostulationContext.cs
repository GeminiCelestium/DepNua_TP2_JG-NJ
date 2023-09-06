using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.API.Models;

namespace ModernRecrut.Postulations.API.Data
{
    public class ModernRecrutPostulationContext : DbContext
    {
        public ModernRecrutPostulationContext(DbContextOptions<ModernRecrutPostulationContext> options) : base(options)
        {

        }

        public DbSet<Postulation> Postulation { get; set; }
        public DbSet<Note> Note { get; set; }
    }
}
