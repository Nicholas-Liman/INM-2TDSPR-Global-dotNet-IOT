using System.Net.Http;
using System.Net.Http.Json;
using AshBoard.Application.DTOs.Leitura;
using AshBoard.Domain.Enums;

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

                    string gravidade;
                    if (leitura.Temperatura >= 50 && leitura.NivelCO2 > 800)
                        gravidade = "Vermelho";
                    else if (leitura.Temperatura >= 39 || leitura.NivelCO2 > 600)
                        gravidade = "Amarelo";
                    else
                        gravidade = "Verde";

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
                            Evento = "Incêndio"
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

            if (fase < 70)
            {
                temperatura = _rnd.NextDouble() * 20 + 15;     // 15–35 °C (Verde)
                co2 = _rnd.Next(400, 600);
            }
            else if (fase < 90)
            {
                temperatura = _rnd.NextDouble() * 11 + 39;     // 39–50 °C (Amarelo)
                co2 = _rnd.Next(601, 800);
            }
            else
            {
                temperatura = _rnd.NextDouble() * 15 + 50;     // 50–65 °C (Vermelho)
                co2 = _rnd.Next(801, 1500);
            }

            var direcao = (DirecaoVento)_rnd.Next(0, 8);

            return new LeituraSensorDto
            {
                SensorId = sensorId,
                Temperatura = Math.Round(temperatura, 1),
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
    }
}
