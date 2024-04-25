


namespace PP.APIServer.Models

{
    public class Turno
    {
        
        public int TurnoId { get; set; }
        public Medico Medico { get; set; }
        public Paciente Paciente { get; set; }
        public DateTime Fecha { get; set; }
    }
}
