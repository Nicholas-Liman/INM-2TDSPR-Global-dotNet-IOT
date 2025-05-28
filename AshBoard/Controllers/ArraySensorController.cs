using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AshBoard.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArraySensorController : ControllerBase
    {
        private readonly IArraySensorService _arraySensorService;

        public ArraySensorController(IArraySensorService arraySensorService)
        {
            _arraySensorService = arraySensorService;
        }

        // GET: api/arraysensor
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os arrays de sensores")]
        [SwaggerResponse(200, "Arrays retornados com sucesso", typeof(List<ArraySensorDto>))]
        public async Task<IActionResult> GetAll()
        {
            var arrays = await _arraySensorService.GetAllAsync();
            return Ok(arrays);
        }

        // GET: api/arraysensor/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar array por ID")]
        [SwaggerResponse(200, "Array encontrado", typeof(ArraySensorDto))]
        [SwaggerResponse(404, "Array não encontrado")]
        public async Task<IActionResult> GetById(int id)
        {
            var array = await _arraySensorService.GetByIdAsync(id);
            if (array == null)
                return NotFound($"ArraySensor com ID {id} não encontrado.");

            return Ok(array);
        }

        // POST: api/arraysensor
        [HttpPost]
        [SwaggerOperation(Summary = "Criar novo array de sensores")]
        [SwaggerResponse(201, "Array criado com sucesso", typeof(ArraySensorDto))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Create([FromBody] CreateArraySensorDto dto)
        {
            try
            {
                var novo = await _arraySensorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = novo.Id }, novo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/arraysensor/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar array de sensores")]
        [SwaggerResponse(200, "Array atualizado com sucesso")]
        [SwaggerResponse(404, "Array não encontrado")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateArraySensorDto dto)
        {
            try
            {
                var atualizado = await _arraySensorService.UpdateAsync(id, dto);
                if (!atualizado)
                    return NotFound($"ArraySensor com ID {id} não encontrado.");

                return Ok($"ArraySensor {id} atualizado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/arraysensor/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover array de sensores")]
        [SwaggerResponse(200, "Array removido com sucesso")]
        [SwaggerResponse(404, "Array não encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _arraySensorService.DeleteAsync(id);
            if (!removido)
                return NotFound($"ArraySensor com ID {id} não encontrado.");

            return Ok($"ArraySensor {id} removido com sucesso.");
        }
    }
}
