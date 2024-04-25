
namespace PP.APIServer.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Especialidad { get; set; }
        public int Telefono { get; set; }

        

        public static implicit operator Medico(string v)
        {
            throw new NotImplementedException();
        }
    }

}
