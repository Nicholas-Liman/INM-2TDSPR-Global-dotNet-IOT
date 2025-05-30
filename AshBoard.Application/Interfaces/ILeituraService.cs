using AshBoard.Application.DTOs.Leitura;
using AshBoard.Domain.Entities;
using System.Threading.Tasks;

namespace AshBoard.Application.Interfaces
{
    public interface ILeituraService
    {
        Task RegistrarLeituraAsync(LeituraSensorDto dto);

        /// <summary>
        /// Simula uma leitura automática do sensor e gera alerta se necessário.
        /// </summary>
        /// <param name="sensor">Sensor com dados simulados</param>
        Task ProcessarLeituraAsync(Sensor sensor);
    }
}
