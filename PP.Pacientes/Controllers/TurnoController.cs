using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PP.APIServer.Models;
using PP.Pacientes.Models;
using System.Data;
using System.Text;

namespace PP.Pacientes.Controllers
{
    public class TurnoController : Controller
    {
        private readonly HttpClient _httpClient;

        public TurnoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7276/api");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Turno/ListarTurnos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var turnos = JsonConvert.DeserializeObject<IEnumerable<TurnoViewModel>>(content);

                return View("Index", turnos);
            }

            return View(new List<TurnoViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Turno turno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(turno);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("/api/Turno/Crear", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Lee el contenido de la respuesta que incluye el ID del turno creado
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var turnoCreado = JsonConvert.DeserializeAnonymousType(responseContent, new { turnoId = 0 });

                        int idTurnoCreado = turnoCreado.turnoId;

                        // Envía el correo electrónico después de crear el turno
                        await EnviarCorreoElectronico(idTurnoCreado);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se pudo crear el turno");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error al crear el turno: {ex.Message}");
                    Console.WriteLine($"Error al crear el turno: {ex.Message}");
                }
            }

            return View();
        }



        private async Task EnviarCorreoElectronico(int idTurno)
        {
            try
            {
                using (var connection = new SqlConnection("Data Source=MARCE-PC;Initial Catalog=PPTECLABTF;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("EnviarCorreoTurno", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idTurno", idTurno);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, $"Error al enviar el correo electrónico: {ex.Message}");
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Turno/ObtenerTurno/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var turno = JsonConvert.DeserializeObject<TurnoViewModel>(content);

                return View(turno);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TurnoViewModel turno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(turno);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"api/Turno/ActualizarTurno?id={id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se ha podido actualizar el turno");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado al actualizar el turno: {ex.Message}");
                Console.WriteLine($"Error inesperado al actualizar el turno: {ex.Message}");
            }

            return View(turno);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Turno/Eliminar?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "No se ha podido eliminar el turno";
                return RedirectToAction("Index");
            }
        }

    }
}
