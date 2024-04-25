using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PP.APIServer.Models;
using Microsoft.AspNetCore.Cors;


namespace PP.APIServer.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {
        private readonly PacienteContext _context;

        public TurnoController(PacienteContext context)
        {
            _context = context;
        }



        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> SacarTurno(Turno turno)
        {
            try
            {
                // Verifica si el Medico y Paciente existen por nombre y apellido
                var medicoExistente = await _context.Medicos
                    .FirstOrDefaultAsync(m => m.Nombre == turno.Medico.Nombre && m.Apellido == turno.Medico.Apellido);

                var pacienteExistente = await _context.Pacientes
                    .FirstOrDefaultAsync(p => p.Nombre == turno.Paciente.Nombre && p.Apellido == turno.Paciente.Apellido);

                if (medicoExistente == null || pacienteExistente == null)
                {
                    return BadRequest("El médico o paciente no existe.");
                }

                // Asigna los objetos existentes al turno
                turno.Medico = medicoExistente;
                turno.Paciente = pacienteExistente;

                // Agrega el turno al contexto y guarda los cambios
                await _context.Turnos.AddAsync(turno);
                await _context.SaveChangesAsync();

                // Devuelve el ID del turno creado
                return Ok(new { turnoId = turno.TurnoId });
            }
            catch (Exception ex)
            {
                // Log del error para debugging
                Console.WriteLine($"Error al sacar turno: {ex.Message}");

                // Devuelve un error 500 con el mensaje de la excepción
                return StatusCode(500, $"Error al sacar turno: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("ListarTurnos")]
        public async Task<ActionResult<IEnumerable<Turno>>> ListarTurnos()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Medico)
                .Include(t => t.Paciente)
                .ToListAsync();

            return Ok(turnos);
        }

        [HttpGet]
        [Route("ObtenerTurno/{id}")]
        public async Task<IActionResult> ObtenerTurno(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Medico)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(t => t.TurnoId == id);

            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);

            if (turno == null)
            {
                return NotFound();
            }

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
