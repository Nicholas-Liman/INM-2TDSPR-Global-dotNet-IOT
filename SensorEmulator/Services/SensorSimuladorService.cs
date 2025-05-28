using SensorEmulator.Models;
using SensorEmulator.Utils;
using System.Net.Http.Json;

namespace SensorEmulator.Services
{
    public class SensorSimuladorService
    {
        private readonly HttpClient _http;
        private readonly Random _random = new();

        private readonly string[] _direcoes =
            { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };

        public SensorSimuladorService(HttpClient http)
        {
            _http = http;
        }

        public async Task ExecutarSimulacaoAsync()
        {
            var sensores = await _http.GetFromJsonAsync<List<SensorDto>>("/api/sensores");

            if (sensores == null || sensores.Count == 0)
            {
                Console.WriteLine("Nenhum sensor encontrado.");
                return;
            }

            foreach (var sensor in sensores)
            {
                var leitura = new LeituraSimuladaDto
                {
                    SensorId = sensor.Id,
                    Temperatura = Math.Round(_random.NextDouble() * (120 - 20) + 20, 1),     // 20–120 °C
                    NivelCO2 = Math.Round(_random.NextDouble() * (2500 - 400) + 400, 2),     // 400–2500 ppm
                    DirecaoVento = _direcoes[_random.Next(_direcoes.Length)],
                    DataHora = DateTime.UtcNow
                };

                try
                {
                    var response = await _http.PostAsJsonAsync("/api/leituras", leitura);
                    Console.WriteLine($"[{DateTime.Now:T}] Sensor {sensor.Id} ➜ {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar leitura para o sensor {sensor.Id}: {ex.Message}");
                }
            }
        }
    }
}
