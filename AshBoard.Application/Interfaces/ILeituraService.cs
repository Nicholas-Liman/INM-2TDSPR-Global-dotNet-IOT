using AshBoard.Application.DTOs.Leitura;

namespace AshBoard.Application.Interfaces
{
    public interface ILeituraService
    {
        Task RegistrarLeituraAsync(LeituraSensorDto dto);
    }
}
