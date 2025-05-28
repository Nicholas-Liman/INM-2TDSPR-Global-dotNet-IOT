using AshBoard.Application.DTOs.Alerta;

namespace AshBoard.Application.Interfaces
{
    public interface IAlertaService
    {
        Task<List<AlertaDto>> GetAllAsync();
        Task<AlertaDto?> GetByIdAsync(int id);
        Task<AlertaDto> CreateAsync(CreateAlertaDto dto);
        Task<bool> UpdateAsync(int id, UpdateAlertaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
