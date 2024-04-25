using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PP.APIServer.Models
{
    public class PacienteContext : DbContext
    {
        // constructor para el dbcontext
        public PacienteContext(DbContextOptions<PacienteContext> options) : base(options)
        {

        }

        // crear tabla Pacientes 
        public DbSet<Paciente> Pacientes { get; set; }

        public DbSet<Medico> Medicos { get; set; }

        // crear tabla Turnos
        public DbSet<Turno> Turnos { get; set; }

        // Método para configurar el DbContext
        public static void Configure(DbContextOptionsBuilder<PacienteContext> builder, IConfiguration configuration)
        {
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

       
    }
}
