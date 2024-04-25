using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PP.APIServer.Models; 
using Microsoft.AspNetCore.Cors;


namespace PP.APIServer.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        // Utiliza el nombre correcto del contexto de base de datos
        private readonly PacienteContext _context;


        //constructor del controlador, donde se inyecta el contexto de la base de datos.
        //Esto nos permite acceder a la base de datos y realizar operaciones CRUD.
        public PacienteController(PacienteContext context)
        {
            _context = context;
        }

        //método CrearPaciente, que recibe un objeto Paciente en el cuerpo de la solicitud HTTP y lo agrega a la base de datos.
        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> CrearPaciente(Paciente paciente)
        {
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            return Ok();
        }


        //método ListarPaciente devuelve una lista de todos los pacientes en la base de datos.
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Paciente>>> ListarPaciente()
        {
            //creamos variable donde guardaremos mediante una consulta a la base de datos mediante metodo To listAsync

            var pacientes = await _context.Pacientes.ToListAsync();
            return Ok(pacientes);
        }


        ////El método FiltrarApellido recibe un parámetro de ruta Apellido y
        //devuelve el primer paciente cuyo apellido contiene el valor proporcionado.
        [HttpGet]
        [Route("FiltrarPorNombre")]
        public async Task<IActionResult> FiltrarPorNombre([FromQuery] string Nombre)
        {
            var pacientesFiltrados = await _context.Pacientes
                .Where(p => p.Nombre.Contains(Nombre))
                .ToListAsync();

            if (pacientesFiltrados.Any())
            {
                return Ok(pacientesFiltrados);
            }

            return NotFound();
        }

        //El método FiltrarApellido recibe un parámetro de ruta Apellido y
        //devuelve el primer paciente cuyo apellido contiene el valor proporcionado.
        [HttpGet]
        [Route("Filtrar por Apellido")]
        public async Task<IActionResult> FiltrarApellido(string Apellido)
        {
            //metodo FirstOrDefaultAsync para buscar por el nombre
            Paciente paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Apellido.Contains(Apellido));

            // Validamos si el paciente es nulo
            if (paciente == null)
            {
                // Si el paciente no se encuentra, devolvemos un NotFound
                return NotFound();
            }

            // Si el paciente se encuentra, devolvemos el paciente
            return Ok(paciente);
        }


        //El método FiltrarPorNAfiliado recibe un parámetro de consulta NAfiliado y
        //devuelve el paciente cuyo número de afiliado coincide con el valor proporcionado.
        [HttpGet]
        [Route("FiltrarPorNAfiliado")]

        public async Task<IActionResult> FiltrarPorNAfiliado([FromQuery] int NAfiliado)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.NAfiliado == NAfiliado);

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(paciente);
        }

        //ActualizarPaciente recibe un parámetro id y un objeto Paciente en el cuerpo de la solicitud HTTP.
        //Busca al paciente correspondiente en la base de datos y actualiza sus propiedades con los valores proporcionados
        [HttpPut]
        [Route("ActualizarPaciente")]
        public async Task<IActionResult> ActualizarPaciente(int id, Paciente paciente)
        {
            var pacienteExistente = await _context.Pacientes.FindAsync(id);

            // Actualizar las propiedades del paciente existente con las del paciente actualizado
            pacienteExistente!.Nombre = paciente.Nombre;
            pacienteExistente.Apellido = paciente.Apellido;
            pacienteExistente.Edad = paciente.Edad;
            pacienteExistente.Email = paciente.Email;
            pacienteExistente.ObraSocial = paciente.ObraSocial;
            pacienteExistente.NAfiliado = paciente.NAfiliado;



            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos



            return Ok();
        }
        //El método ObtenerPaciente recibe un parámetro id y devuelve el paciente correspondiente en la base de datos.
        [HttpGet]
        [Route("ObtenerPaciente/{id}")]
        public async Task<IActionResult> ObtenerPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(paciente);
        }

        //El método Eliminar recibe un parámetro id y elimina al paciente correspondiente de la base de datos.

        [HttpDelete]
        [Route("Eliminar")]

        public async Task<IActionResult> Eliminar(int id)
        {
            // crear variable  llamamos a la base de datos y traemos todos los registros de id relacionados 
            var pacienteBorrado = await _context.Pacientes.FindAsync(id);
            // una vez que tengamos el id aplicamos metodo context remove y removemos el objeto que guardamos en la var pacienteBorrado

            _context.Pacientes.Remove(pacienteBorrado!);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}