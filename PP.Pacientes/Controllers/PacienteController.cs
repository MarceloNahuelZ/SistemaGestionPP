using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PP.APIServer.Models;
using PP.Pacientes.Models;
using System.Text;

namespace PP.Pacientes.Controllers
{
    public class PacienteController : Controller
    {
        private readonly HttpClient _httpClient;

        public PacienteController(IHttpClientFactory httpClientFactory)
        {
            // Crear una instancia de HttpClient utilizando  HttpClient
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7276/api");
        }

        public async Task<IActionResult> Index()
        {
            // Realizar una solicitud GET a la API para obtener la lista de pacientes
            var response = await _httpClient.GetAsync("api/Paciente/Listar");

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var content = await response.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en una lista de objetos PacienteViewModel
                var pacientes = JsonConvert.DeserializeObject<IEnumerable<PacienteViewModel>>(content);

                // Devolver la vista "Index" con la lista de pacientes como modelo
                return View("Index", pacientes);
            }

            // Si la solicitud no fue exitosa, devolver la vista "Index" con una lista vacía de pacientes
            return View(new List<PacienteViewModel>());
        }

        public IActionResult Create()
        {
            // Devolver la vista "Create"
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                // Serializar el objeto Paciente en formato JSON
                var json = JsonConvert.SerializeObject(paciente);
                // Crear un objeto StringContent con el JSON y el tipo de contenido
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar una solicitud POST a la API para crear un nuevo paciente
                var response = await _httpClient.PostAsync("/api/Paciente/Crear", content);

                if (response.IsSuccessStatusCode)
                {
                    // Redirigir al método "Index" después de crear el paciente exitosamente
                    return RedirectToAction("Index");
                }
                else
                {
                    // Agregar un error al ModelState si no se pudo crear el paciente
                    ModelState.AddModelError(string.Empty, "No se pudo crear el paciente");
                }
            }
            // Devolver la vista "Create" con el modelo Paciente
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Realizar una solicitud GET a la API para obtener los detalles de un paciente específico
            var response = await _httpClient.GetAsync($"api/Paciente/ObtenerPaciente/{id}");
            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var content = await response.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en un objeto PacienteViewModel
                var paciente = JsonConvert.DeserializeObject<PacienteViewModel>(content);

                // Devolver la vista "Edit" con el objeto PacienteViewModel como modelo
                return View(paciente);
            }
            else
            {
                // Redirigir al método "Index" si no se pudo obtener el paciente
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PacienteViewModel paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Serializar el objeto PacienteViewModel en formato JSON
                    var json = JsonConvert.SerializeObject(paciente);
                    // Crear un objeto StringContent con el JSON y el tipo de contenido
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Realizar una solicitud PUT a la API para actualizar un paciente existente
                    var response = await _httpClient.PutAsync($"api/Paciente/ActualizarPaciente?id={id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Redirigir al método "Index" después de actualizar el paciente exitosamente
                        return RedirectToAction("Index", new { id });
                    }
                    else
                    {
                        // Agregar un error al ModelState si no se pudo actualizar el paciente
                        ModelState.AddModelError(string.Empty, "No se ha podido actualizar el paciente");
                    }
                }
            }
            catch (Exception ex)
            {
                // Agregar un error al ModelState si se produce una excepción inesperada
                ModelState.AddModelError(string.Empty, $"Error inesperado al actualizar el paciente: {ex.Message}");
                Console.WriteLine($"Error inesperado al actualizar el paciente: {ex.Message}");
            }

            // Devolver la vista "Edit" con el modelo PacienteViewModel
            return View(paciente);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Realizar una solicitud DELETE a la API para eliminar un paciente específico
            var response = await _httpClient.DeleteAsync($"api/Paciente/Eliminar?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                // Redirigir al método "Index" si no se pudo eliminar el paciente
                return RedirectToAction("Index");
            }
            else
            {
                // Establecer un mensaje de error en TempData si no se pudo eliminar el paciente
                TempData["Error"] = "No se ha podido eliminar el paciente";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Filtrar(string filtro)
        {
            // Realizar una solicitud GET a la API para filtrar los pacientes por nombre
            var response = await _httpClient.GetAsync($"api/Paciente/FiltrarPorNombre?Nombre={filtro}");

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var content = await response.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en una lista de objetos PacienteViewModel
                var pacientesFiltrados = JsonConvert.DeserializeObject<IEnumerable<PacienteViewModel>>(content);

                // Devolver la vista "Index" con la lista de pacientes filtrados como modelo
                return View("Index", pacientesFiltrados);
            }

            // Si la solicitud no fue exitosa, realizar una solicitud GET para obtener todos los pacientes
            var responseAll = await _httpClient.GetAsync("api/Paciente/Listar");

            if (responseAll.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var contentAll = await responseAll.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en una lista de objetos PacienteViewModel
                var pacientes = JsonConvert.DeserializeObject<IEnumerable<PacienteViewModel>>(contentAll);

                // Devolver la vista "Index" con la lista de todos los pacientes como modelo
                return View("Index", pacientes);
            }

            // Si la solicitud no fue exitosa, devolver la vista "Index" con una lista vacía de pacientes
            return View("Index", new List<PacienteViewModel>());
        }

        [HttpGet]
        [Route("FiltrarPorNombre")]
        public async Task<IActionResult> FiltrarPorNombre([FromQuery] string Nombre)
        {
            // Realizar una solicitud GET a la API para filtrar los pacientes por nombre
            var pacientesFiltrados = await _httpClient.GetAsync($"api/Paciente/FiltrarPorNombre?Nombre={Nombre}");

            if (pacientesFiltrados.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var content = await pacientesFiltrados.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en una lista de objetos PacienteViewModel
                var pacientes = JsonConvert.DeserializeObject<IEnumerable<PacienteViewModel>>(content);
                return Ok(pacientes);
            }

            // Devolver un resultado NotFound si no se encontraron pacientes filtrados
            return NotFound();
        }
    }
}
