using AshBoard.Application.DTOs.Alerta;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Interfaces
{
    public interface IAlertaService
    {
        /// <summary>
        /// Retorna todos os alertas com dados de sensor, localização e predição.
        /// </summary>
        Task<List<AlertaDto>> GetAllAsync();

        /// <summary>
        /// Retorna um alerta específico pelo ID.
        /// </summary>
        Task<AlertaDto?> GetByIdAsync(int id);

        /// <summary>
        /// Cria um novo alerta a partir de um DTO com informações do sensor e predição.
        /// </summary>
        Task<AlertaDto> CreateAsync(CreateAlertaDto dto);

        /// <summary>
        /// Exclui um alerta existente com base no ID.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
