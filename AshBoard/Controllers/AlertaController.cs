using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AshBoard.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3 - Alertas")]
    public class AlertaController : ControllerBase
    {
        private readonly IAlertaService _alertaService;

        public AlertaController(IAlertaService alertaService)
        {
            _alertaService = alertaService;
        }

        // GET: api/alerta
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os alertas")]
        [SwaggerResponse(200, "Lista de alertas retornada com sucesso", typeof(List<AlertaDto>))]
        public async Task<IActionResult> GetAll()
        {
            var alertas = await _alertaService.GetAllAsync();
            return Ok(alertas);
        }

        // GET: api/alerta/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar alerta por ID")]
        [SwaggerResponse(200, "Alerta encontrado", typeof(AlertaDto))]
        [SwaggerResponse(404, "Alerta não encontrado")]
        public async Task<IActionResult> GetById(int id)
        {
            var alerta = await _alertaService.GetByIdAsync(id);
            if (alerta == null)
                return NotFound($"Alerta com ID {id} não encontrado.");

            return Ok(alerta);
        }

        // POST: api/alerta
        [HttpPost]
        [SwaggerOperation(Summary = "Criar um novo alerta")]
        [SwaggerResponse(201, "Alerta criado com sucesso", typeof(AlertaDto))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Create([FromBody] CreateAlertaDto dto)
        {
            try
            {
                var alerta = await _alertaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, alerta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/alerta/{id} → Regra: alertas não podem ser atualizados
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualização de alerta desativada por regra de negócio")]
        [SwaggerResponse(400, "Atualizações não são permitidas")]
        public IActionResult Update(int id, [FromBody] object dto)
        {
            return BadRequest("Atualizações em alertas não são permitidas por regra de negócio. Crie um novo alerta.");
        }

        // DELETE: api/alerta/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover um alerta")]
        [SwaggerResponse(200, "Alerta removido com sucesso")]
        [SwaggerResponse(404, "Alerta não encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _alertaService.DeleteAsync(id);
            if (!removido)
                return NotFound($"Alerta com ID {id} não encontrado.");

            return Ok($"Alerta {id} removido com sucesso.");
        }
    }
}
