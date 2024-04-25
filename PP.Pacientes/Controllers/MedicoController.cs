using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PP.APIServer.Models;
using PP.Pacientes.Models;
using System.Text;

namespace PP.Pacientes.Controllers
{
    public class MedicoController : Controller
    {
        private readonly HttpClient _httpClient;

        public MedicoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7276/api");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Medico/Listar");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var medicos = JsonConvert.DeserializeObject<IEnumerable<MedicoViewModel>>(content);

                return View("Index", medicos);
            }

            return View(new List<MedicoViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Medico medico)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(medico);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Medico/Crear", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el médico");
                }
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Medico/Eliminar?id={id}");
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "No se ha podido eliminar al medico";

                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Realizar una solicitud GET a la API para obtener los detalles de un paciente específico
            var response = await _httpClient.GetAsync($"api/Medico/ObtenerMedico/{id}");
            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena
                var content = await response.Content.ReadAsStringAsync();
                // Deserializar el contenido JSON en un objeto PacienteViewModel
                var medico = JsonConvert.DeserializeObject<MedicoViewModel>(content);

                // Devolver la vista "Edit" con el objeto PacienteViewModel como modelo
                return View(medico);
            }
            else
            {
                // Redirigir al método "Index" si no se pudo obtener el paciente
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MedicoViewModel medico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Serializar el objeto PacienteViewModel en formato JSON
                    var json = JsonConvert.SerializeObject(medico);
                    // Crear un objeto StringContent con el JSON y el tipo de contenido
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Realizar una solicitud PUT a la API para actualizar un paciente existente
                    var response = await _httpClient.PutAsync($"api/Medico/ActualizarMedico?id={id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Redirigir al método "Index" después de actualizar el paciente exitosamente
                        return RedirectToAction("Index", new { id });
                    }
                    else
                    {
                        // Agregar un error al ModelState si no se pudo actualizar el paciente
                        ModelState.AddModelError(string.Empty, "No se ha podido actualizar el Medico");
                    }
                }
            }
            catch (Exception ex)
            {
                // Agregar un error al ModelState si se produce una excepción inesperada
                ModelState.AddModelError(string.Empty, $"Error inesperado al actualizar el medico: {ex.Message}");
                Console.WriteLine($"Error inesperado al actualizar el medico: {ex.Message}");
            }

            // Devolver la vista "Edit" con el modelo PacienteViewModel
            return View(medico);
        }


    }
}

