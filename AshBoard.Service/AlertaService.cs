using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Repositories;
using AshBoard.Application.Interfaces;
using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AshBoard.Infrastructure.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly IAlertaRepository _alertaRepository;

        public AlertaService(IAlertaRepository alertaRepository)
        {
            _alertaRepository = alertaRepository;
        }

        public async Task<List<AlertaDto>> GetAllAsync()
        {
            var alertas = await _alertaRepository.GetAllAsync();
            return alertas.Select(a => new AlertaDto
            {
                Id = a.Id,
                Nivel = a.Nivel,
                Mensagem = a.Mensagem
            }).ToList();
        }

        public async Task<AlertaDto> GetByIdAsync(int id)
        {
            var a = await _alertaRepository.GetByIdAsync(id);
            if (a == null) return null;

            return new AlertaDto
            {
                Id = a.Id,
                Nivel = a.Nivel,
                Mensagem = a.Mensagem
            };
        }

        public async Task CreateAsync(CreateAlertaDto dto)
        {
            var entity = new AlertaEntity
            {
                Nivel = dto.Nivel,
                Mensagem = dto.Mensagem
            };
            await _alertaRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, UpdateAlertaDto dto)
        {
            var alerta = await _alertaRepository.GetByIdAsync(id);
            if (alerta == null) return;

            alerta.Nivel = dto.Nivel;
            alerta.Mensagem = dto.Mensagem;

            await _alertaRepository.UpdateAsync(alerta);
        }

        public async Task DeleteAsync(int id)
        {
            await _alertaRepository.DeleteAsync(id);
        }
    }
}
