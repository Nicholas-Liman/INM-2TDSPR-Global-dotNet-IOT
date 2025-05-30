using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AshBoard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISensorService _sensorService;
        private readonly IArraySensorService _arraySensorService;

        public DashboardController(ISensorService sensorService, IArraySensorService arraySensorService)
        {
            _sensorService = sensorService;
            _arraySensorService = arraySensorService;
        }

        // Lista todos os sensores ordenados por ID
        public async Task<IActionResult> Index()
        {
            var sensores = await _sensorService.GetAllAsync();
            var ordenados = sensores.OrderBy(s => s.Id).ToList();
            return View(ordenados);
        }

        // Lista todos os arrays com seus sensores
        public async Task<IActionResult> ArraySensors()
        {
            var arrays = await _arraySensorService.GetAllAsync();
            return View(arrays);
        }

        // Página de criação de sensor
        public IActionResult CreateSensor() => View();

        // Cria um novo sensor
        [HttpPost]
        public async Task<IActionResult> CreateSensor(CreateSensorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _sensorService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        // Página de edição
        public async Task<IActionResult> EditSensor(string id)
        {
            var sensor = await _sensorService.GetByIdAsync(id);
            if (sensor == null)
                return NotFound();

            return View(sensor);
        }

        // Edita um sensor existente
        [HttpPost]
        public async Task<IActionResult> EditSensor(SensorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var update = new UpdateSensorDto
            {
                NomeLocal = dto.NomeLocal,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            var success = await _sensorService.UpdateAsync(dto.Id, update);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }

        // Deleta um sensor
        public async Task<IActionResult> DeleteSensor(string id)
        {
            var success = await _sensorService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
