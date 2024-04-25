using PP.APIServer.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PP.Pacientes.Models
{
    public class TurnoViewModel
    {
        public int TurnoId { get; set; }
        public Medico Medico { get; set; }
        public Paciente Paciente { get; set; }
        public DateTime Fecha { get; set; }
    }
}
