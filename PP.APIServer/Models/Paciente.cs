
namespace PP.APIServer.Models
{

    //Entidad 
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Email { get; set; }
        public string ObraSocial { get; set; }
        public int NAfiliado { get; set; }

        public static implicit operator Paciente(string v)
        {
            throw new NotImplementedException();
        }
    }
}
