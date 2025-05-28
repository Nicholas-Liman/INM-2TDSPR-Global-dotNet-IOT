using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var sensores = await _sensorService.GetAllAsync();
            var ordenados = sensores.OrderBy(s => s.Id).ToList();
            return View(ordenados);
        }

        public async Task<IActionResult> ArraySensors()
        {
            var arrays = await _arraySensorService.GetAllAsync();
            return View(arrays);
        }

        public IActionResult CreateSensor() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSensor(CreateSensorDto dto)
        {
            await _sensorService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditSensor(int id)
        {
            var sensor = await _sensorService.GetByIdAsync(id);
            return View(sensor);
        }

        [HttpPost]
        public async Task<IActionResult> EditSensor(SensorDto dto)
        {
            var update = new UpdateSensorDto
            {
                Nome = dto.Nome,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            await _sensorService.UpdateAsync(dto.Id, update);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSensor(int id)
        {
            await _sensorService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
