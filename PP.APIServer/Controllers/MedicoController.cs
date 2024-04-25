using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PP.APIServer.Models;
using Microsoft.AspNetCore.Cors;

namespace PP.APIServer.Controllers
{

    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly PacienteContext _context;

        public MedicoController(PacienteContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicos()
        {
            return await _context.Medicos.ToListAsync();
        }


        [HttpGet]
        [Route("ObtenerMedico/{id}")]
        public async Task<ActionResult<Medico>> ObtenerMedico(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)
            {
                return NotFound();
            }

            return Ok(medico);
        }


        [HttpPut]
        [Route("ActualizarMedico")]
        public async Task<IActionResult> ActualizarMedico(int id, Medico medico)
        {
            var medicoExistente = await _context.Medicos.FindAsync(id);

            medicoExistente!.Nombre = medico.Nombre;
            medicoExistente.Apellido = medico.Apellido;
            medicoExistente.Especialidad = medico.Especialidad;
            medicoExistente.Telefono = medico.Telefono;

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult<Medico>> CrearMedico(Medico medico)
        {
            await _context.Medicos.AddAsync(medico);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> EliminarMedico(int id)
        {
            var medicoBorrado = await _context.Medicos.FindAsync(id);

            _context.Medicos.Remove(medicoBorrado);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

