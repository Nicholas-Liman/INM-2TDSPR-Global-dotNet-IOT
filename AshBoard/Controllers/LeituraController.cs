using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AshBoard.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeiturasController : ControllerBase
    {
        private readonly ILeituraService _leituraService;

        public LeiturasController(ILeituraService leituraService)
        {
            _leituraService = leituraService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Registrar uma leitura de sensor")]
        [SwaggerResponse(200, "Leitura registrada com sucesso")]
        [SwaggerResponse(400, "Erro na requisição")]
        public async Task<IActionResult> Registrar([FromBody] LeituraSensorDto dto)
        {
            try
            {
                await _leituraService.RegistrarLeituraAsync(dto);
                return Ok("Leitura registrada e alerta avaliado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}