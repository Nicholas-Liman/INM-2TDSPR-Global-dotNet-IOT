using System.Net.Http;
using System.Net.Http.Json;
using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.DTOs.Alerta;
using AshBoard.Domain.Enums;
using AshBoard.Data.ML;

namespace SensorEmulator.Services
{
    public class SensorSimulatorService
    {
        private readonly HttpClient _http;
        private readonly Random _rnd = new();

        public SensorSimulatorService(HttpClient http)
        {
            _http = http;
        }

        public async Task AtualizarSensoresAsync()
        {
            try
            {
                var sensores = await _http.GetFromJsonAsync<List<SensorDto>>("/api/sensor");

                if (sensores == null || sensores.Count == 0)
                {
                    Console.WriteLine("Nenhum sensor encontrado.");
                    return;
                }

                foreach (var sensor in sensores)
                {
                    var leitura = GerarLeitura(sensor.Id);

                    var response = await _http.PutAsJsonAsync($"/api/sensor/{sensor.Id}/leitura", leitura);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Erro ao atualizar sensor {sensor.Id}: {response.StatusCode}");
                        continue;
                    }

                    // === Integração com IA ===
                    var resultadoML = MLModelFacade.Predict(leitura.Temperatura, leitura.NivelCO2);
                    float probabilidade = resultadoML.Probability * 100;

                    // === Gravidade baseada em regras físicas ===
                    string gravidade;
                    if (leitura.Temperatura >= 50 && leitura.NivelCO2 > 1200)
                        gravidade = "Vermelho";
                    else if (leitura.Temperatura >= 41 && leitura.NivelCO2 > 1000)
                        gravidade = "Amarelo";
                    else
                        gravidade = "Verde";

                    Console.WriteLine($"[ML] Sensor {sensor.Id}: Probabilidade de incêndio: {probabilidade:F2}%");

                    if (gravidade != "Verde")
                    {
                        var alerta = new CreateAlertaDto
                        {
                            SensorId = sensor.Id,
                            DataHoraColeta = DateTime.UtcNow,
                            NomeLocal = "Simulação",
                            Latitude = sensor.Latitude,
                            Longitude = sensor.Longitude,
                            Gravidade = gravidade,
                            Evento = "Incêndio",
                            Temperatura = leitura.Temperatura,
                            NivelCO2 = leitura.NivelCO2,
                            ProbabilidadeIncendio = probabilidade,
                            Observacao = $"Chance de Incêndio: {probabilidade:F1}%"
                        };

                        await _http.PostAsJsonAsync("/api/alerta", alerta);
                    }

                    Console.WriteLine($"Sensor {sensor.Id}: T={leitura.Temperatura}°C | CO₂={leitura.NivelCO2} ppm | Vento={leitura.DirecaoVento} → Alerta: {gravidade}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar sensores: {ex.Message}");
            }
        }

        private LeituraSensorDto GerarLeitura(string sensorId)
        {
            double temperatura;
            int co2;

            var fase = _rnd.Next(0, 100);

            if (fase < 85)
            {
                // Verde: 15–40 °C, CO₂: 400–1000 ppm
                temperatura = _rnd.NextDouble() * (40 - 15) + 15;
                co2 = _rnd.Next(400, 1001);
            }
            else if (fase < 97)
            {
                // Amarelo: 41–49 °C, CO₂: 1001–1200 ppm
                temperatura = _rnd.NextDouble() * (49 - 41) + 41;
                co2 = _rnd.Next(1001, 1201);
            }
            else
            {
                // Vermelho: 50–65 °C, CO₂: 1201–1500 ppm
                temperatura = _rnd.NextDouble() * 15 + 50;
                co2 = _rnd.Next(1201, 1501);
            }

            var direcao = (DirecaoVento)_rnd.Next(0, 8);

            return new LeituraSensorDto
            {
                SensorId = sensorId,
                Temperatura = (float)Math.Round(temperatura, 1),
                NivelCO2 = co2,
                DirecaoVento = direcao.ToString(),
                DataHora = DateTime.UtcNow
            };
        }
    }

    public class SensorDto
    {
        public string Id { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateAlertaDto
    {
        public DateTime DataHoraColeta { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde";
        public string SensorId { get; set; } = string.Empty;
        public string? Observacao { get; set; }

        // ML.NET
        public float Temperatura { get; set; }
        public float NivelCO2 { get; set; }
        public float ProbabilidadeIncendio { get; set; }
    }
}
