using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Infrastructure.Services
{
    public class LeituraService : ILeituraService
    {
        private readonly AshBoardDbContext _context;

        public LeituraService(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarLeituraAsync(LeituraSensorDto dto)
        {
            var sensor = await _context.Sensores.FindAsync(dto.SensorId);
            if (sensor == null)
                throw new ArgumentException("Sensor não encontrado.");

            // Atualiza os dados do sensor
            sensor.Temperatura = dto.Temperatura;
            sensor.NivelCO2 = dto.NivelCO2;
            if (Enum.TryParse(dto.DirecaoVento, out DirecaoVento direcao))
            {
                sensor.DirecaoVento = (int)direcao;
            }

            // Avalia condições para gerar alerta
            var alerta = new Alerta
            {
                DataHoraColeta = dto.DataHora,
                NomeLocal = sensor.NomeLocal,
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude,
                Evento = "Incêndio",
                SensorId = sensor.Id
            };

            // Temperatura
            if (dto.Temperatura < 36)
            {
                alerta.Gravidade = "Verde";
                alerta.IncendioProximo = false;
            }
            else if (dto.Temperatura < 50)
            {
                alerta.Gravidade = "Amarelo";
                alerta.IncendioProximo = true;
            }
            else
            {
                alerta.Gravidade = "Vermelho";
                alerta.IncendioProximo = true;
            }

            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();
        }
    }
}
