using Agenda.Server.Models.Agenda;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Server.Data
{
    public class AgendaContext : DbContext
    {
        public AgendaContext(DbContextOptions<AgendaContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Contato> Contato { get; set; }

        public DbSet<Compromisso> Compromisso { get; set; }
    }
}
