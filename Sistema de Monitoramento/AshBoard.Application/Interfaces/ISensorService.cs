using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.Leitura;

namespace AshBoard.Application.Interfaces
{
    public interface ISensorService
    {
        Task<List<SensorDto>> GetAllAsync();
        Task<SensorDto?> GetByIdAsync(string id);
        Task<SensorDto> CreateAsync(CreateSensorDto dto);
        Task<bool> UpdateAsync(string id, UpdateSensorDto dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> AtualizarLeituraAsync(string id, LeituraSensorDto dto);
    }
}
