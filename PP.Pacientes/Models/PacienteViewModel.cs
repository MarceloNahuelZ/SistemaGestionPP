using System.ComponentModel;

namespace PP.Pacientes.Models
{
    public class PacienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Email { get; set; }

        [DisplayName("Obra Social")] //con el Display Name cambiamos el encabezado que se mostrara en pantalla
        public string ObraSocial { get; set; }

        [DisplayName("Afiliado")]
        public int NAfiliado { get; set; }
    }
}
