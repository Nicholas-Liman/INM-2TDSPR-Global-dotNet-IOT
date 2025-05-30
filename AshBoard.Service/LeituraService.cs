using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Enums;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Service.Services
{
    public class LeituraService : ILeituraService
    {
        private readonly AshBoardDbContext _context;
        private readonly IAlertaMLService _mlService;

        public LeituraService(AshBoardDbContext context, IAlertaMLService mlService)
        {
            _context = context;
            _mlService = mlService;
        }

        public async Task RegistrarLeituraAsync(LeituraSensorDto dto)
        {
            var sensor = await _context.Sensores.FindAsync(dto.SensorId);
            if (sensor == null)
                throw new ArgumentException("Sensor não encontrado.");

            sensor.Temperatura = dto.Temperatura;
            sensor.NivelCO2 = dto.NivelCO2;
            sensor.DirecaoVento = dto.DirecaoVento;
            sensor.DataUltimaLeitura = dto.DataHora;

            var alerta = GerarAlerta(dto.DataHora, sensor);
            _context.Alertas.Add(alerta);

            await _context.SaveChangesAsync();
        }

        public async Task ProcessarLeituraAsync(Sensor sensor)
        {
            sensor.Temperatura = SimularTemperatura();
            sensor.NivelCO2 = SimularNivelCO2();
            sensor.DirecaoVento = SimularDirecao().ToString();
            sensor.DataUltimaLeitura = DateTime.Now;

            var alerta = GerarAlerta(sensor.DataUltimaLeitura.Value, sensor);
            _context.Alertas.Add(alerta);

            await _context.SaveChangesAsync();
        }

        private Alerta GerarAlerta(DateTime dataHora, Sensor sensor)
        {
            var alerta = new Alerta
            {
                DataHoraColeta = dataHora,
                NomeLocal = sensor.NomeLocal,
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude,
                Evento = "Incêndio",
                SensorId = sensor.Id
            };

            if (sensor.Temperatura >= 50 && sensor.NivelCO2 > 800)
            {
                alerta.Gravidade = "Vermelho";
            }
            else if (sensor.Temperatura >= 39 || sensor.NivelCO2 > 600)
            {
                alerta.Gravidade = "Amarelo";

                // Obter probabilidade da IA
                float temperatura = (float)sensor.Temperatura;
                float nivelCO2 = (float)sensor.NivelCO2;

                float probabilidade = _mlService.ObterProbabilidadeIncendio(temperatura, nivelCO2);

                // Formatando como porcentagem
                alerta.Observacao = $"Chance estimada de incêndio: {probabilidade:P1}";
            }
            else
            {
                alerta.Gravidade = "Verde";
            }

            return alerta;
        }

        private double SimularTemperatura()
        {
            var random = new Random();
            return Math.Round(15 + random.NextDouble() * 60, 1);
        }

        private double SimularNivelCO2()
        {
            var random = new Random();
            return Math.Round(400 + random.NextDouble() * 1200, 2);
        }

        private DirecaoVento SimularDirecao()
        {
            var valores = Enum.GetValues(typeof(DirecaoVento));
            var random = new Random();
            return (DirecaoVento)valores.GetValue(random.Next(valores.Length))!;
        }
    }
}
