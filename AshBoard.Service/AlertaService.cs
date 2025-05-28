using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Infrastructure.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly AshBoardDbContext _context;

        public AlertaService(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<AlertaDto> CreateAsync(CreateAlertaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NomeLocal) || dto.NomeLocal.Length > 50)
                throw new ArgumentException("NomeLocal é obrigatório e deve ter até 50 caracteres.");

            var sensor = await _context.Sensores.FindAsync(dto.SensorId);
            if (sensor == null)
                throw new ArgumentException("Sensor associado não encontrado.");

            var alerta = new Alerta
            {
                DataHoraColeta = dto.DataHoraColeta,
                NomeLocal = dto.NomeLocal,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Evento = dto.Evento,
                Gravidade = dto.Gravidade,
                IncendioProximo = dto.IncendioProximo,
                SensorId = dto.SensorId
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
                IncendioProximo = alerta.IncendioProximo,
                SensorId = alerta.SensorId
            };
        }

        public async Task<List<AlertaDto>> GetAllAsync()
        {
            return await _context.Alertas
                .Select(a => new AlertaDto
                {
                    Id = a.Id,
                    DataHoraColeta = a.DataHoraColeta,
                    NomeLocal = a.NomeLocal,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Evento = a.Evento,
                    Gravidade = a.Gravidade,
                    IncendioProximo = a.IncendioProximo,
                    SensorId = a.SensorId
                })
                .ToListAsync();
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
                IncendioProximo = alerta.IncendioProximo,
                SensorId = alerta.SensorId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateAlertaDto dto)
        {
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null) return false;

            alerta.Gravidade = dto.Gravidade;
            alerta.IncendioProximo = dto.IncendioProximo;

            await _context.SaveChangesAsync();
            return true;
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
