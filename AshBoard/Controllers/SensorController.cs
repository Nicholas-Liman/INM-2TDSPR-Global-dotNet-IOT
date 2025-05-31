using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AshBoard.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "1 - Sensor")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        // GET: api/sensor
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os sensores")]
        [SwaggerResponse(200, "Sensores retornados com sucesso", typeof(List<SensorDto>))]
        public async Task<IActionResult> GetAll()
        {
            var sensores = await _sensorService.GetAllAsync();
            return Ok(sensores);
        }

        // GET: api/sensor/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar um sensor por ID")]
        [SwaggerResponse(200, "Sensor encontrado", typeof(SensorDto))]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> GetById(string id)
        {
            var sensor = await _sensorService.GetByIdAsync(id);
            if (sensor == null)
                return NotFound($"Sensor com ID '{id}' não encontrado.");

            return Ok(sensor);
        }

        // POST: api/sensor
        [HttpPost]
        [SwaggerOperation(Summary = "Criar um novo sensor")]
        [SwaggerResponse(201, "Sensor criado com sucesso", typeof(SensorDto))]
        [SwaggerResponse(400, "Dados inválidos ou duplicados")]
        public async Task<IActionResult> Create([FromBody] CreateSensorDto dto)
        {
            try
            {
                var novoSensor = await _sensorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = novoSensor.Id }, novoSensor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/sensor/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar um sensor existente")]
        [SwaggerResponse(200, "Sensor atualizado com sucesso")]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateSensorDto dto)
        {
            try
            {
                var atualizado = await _sensorService.UpdateAsync(id, dto);
                if (!atualizado)
                    return NotFound($"Sensor com ID '{id}' não encontrado.");

                return Ok($"Sensor {id} atualizado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/sensor/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover um sensor")]
        [SwaggerResponse(200, "Sensor removido com sucesso")]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> Delete(string id)
        {
            var removido = await _sensorService.DeleteAsync(id);
            if (!removido)
                return NotFound($"Sensor com ID '{id}' não encontrado.");

            return Ok($"Sensor {id} removido com sucesso.");
        }

        // PUT: /api/sensor/{id}/leitura
        [HttpPut("{id}/leitura")]
        [SwaggerOperation(Summary = "Atualizar leitura do sensor")]
        [SwaggerResponse(200, "Leitura atualizada com sucesso")]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> AtualizarLeitura(string id, [FromBody] LeituraSensorDto dto)
        {
            var atualizado = await _sensorService.AtualizarLeituraAsync(id, dto);
            if (!atualizado)
                return NotFound($"Sensor com ID '{id}' não encontrado.");

            return Ok($"Leitura do sensor {id} atualizada com sucesso.");
        }
    }
}
