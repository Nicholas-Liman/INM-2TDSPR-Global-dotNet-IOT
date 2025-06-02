using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Service.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly AshBoardDbContext _context;
        private readonly IAlertaMLService _mlService;

        public AlertaService(AshBoardDbContext context, IAlertaMLService mlService)
        {
            _context = context;
            _mlService = mlService;
        }

        public async Task<List<AlertaDto>> GetAllAsync()
        {
            var alertas = await _context.Alertas
                .Include(a => a.Sensor)
                .OrderByDescending(a => a.DataHoraColeta)
                .ToListAsync();

            return alertas.Select(alerta => new AlertaDto
            {
                Id = alerta.Id,
                DataHoraColeta = alerta.DataHoraColeta,
                NomeLocal = alerta.NomeLocal,
                Latitude = alerta.Latitude,
                Longitude = alerta.Longitude,
                Evento = alerta.Evento,
                Gravidade = alerta.Gravidade,
                SensorId = alerta.SensorId,
                Observacao = alerta.Observacao,
                Temperatura = alerta.Temperatura,
                NivelCO2 = alerta.NivelCO2,
                ProbabilidadeIncendio = alerta.ProbabilidadeIncendio
            }).ToList();
        }

        public async Task<AlertaDto?> GetByIdAsync(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null) return null;

            return new AlertaDto
            {
                Id = alerta.Id,
                DataHoraColeta = alerta.DataHoraColeta,
                NomeLocal = alerta.NomeLocal,
                Latitude = alerta.Latitude,
                Longitude = alerta.Longitude,
                Evento = alerta.Evento,
                Gravidade = alerta.Gravidade,
                SensorId = alerta.SensorId,
                Observacao = alerta.Observacao,
                Temperatura = alerta.Temperatura,
                NivelCO2 = alerta.NivelCO2,
                ProbabilidadeIncendio = alerta.ProbabilidadeIncendio
            };
        }

        public async Task<AlertaDto> CreateAsync(CreateAlertaDto dto)
        {
            float probabilidade = 0f;
            string? observacao = dto.Observacao;

            if (dto.Gravidade == "Amarelo")
            {
                probabilidade = _mlService.ObterProbabilidadeIncendio(dto.Temperatura, dto.NivelCO2);
                observacao = $"Chance de Incêndio: {probabilidade:F1}%";
            }
            else if (dto.Gravidade == "Vermelho")
            {
                probabilidade = 100f;
                observacao = "Chance de Incêndio: 100%";
            }

            var alerta = new Alerta
            {
                DataHoraColeta = dto.DataHoraColeta,
                NomeLocal = dto.NomeLocal,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Evento = dto.Evento,
                Gravidade = dto.Gravidade,
                SensorId = dto.SensorId,
                Observacao = observacao,
                Temperatura = dto.Temperatura,
                NivelCO2 = dto.NivelCO2,
                ProbabilidadeIncendio = probabilidade
            };

            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();

            return new AlertaDto
            {
                Id = alerta.Id,
                DataHoraColeta = alerta.DataHoraColeta,
                NomeLocal = alerta.NomeLocal,
                Latitude = alerta.Latitude,
                Longitude = alerta.Longitude,
                Evento = alerta.Evento,
                Gravidade = alerta.Gravidade,
                SensorId = alerta.SensorId,
                Observacao = alerta.Observacao,
                Temperatura = alerta.Temperatura,
                NivelCO2 = alerta.NivelCO2,
                ProbabilidadeIncendio = alerta.ProbabilidadeIncendio
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null) return false;

            _context.Alertas.Remove(alerta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
