using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AshBoard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISensorService _sensorService;
        private readonly IArraySensorService _arraySensorService;
        private readonly IAlertaService _alertaService;

        public DashboardController(
            ISensorService sensorService,
            IArraySensorService arraySensorService,
            IAlertaService alertaService)
        {
            _sensorService = sensorService;
            _arraySensorService = arraySensorService;
            _alertaService = alertaService;
        }

        // SENSORES

        public async Task<IActionResult> Index()
        {
            var sensores = await _sensorService.GetAllAsync();
            var ordenados = sensores.OrderBy(s => s.Id).ToList();
            return View(ordenados);
        }

        public IActionResult CreateSensor() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSensor(CreateSensorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _sensorService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditSensor(string id)
        {
            var sensor = await _sensorService.GetByIdAsync(id);
            if (sensor == null)
                return NotFound();

            return View(sensor);
        }

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

        public async Task<IActionResult> DeleteSensor(string id)
        {
            var success = await _sensorService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }

        // ARRAYS DE SENSORES

        public async Task<IActionResult> ArraySensors()
        {
            var arrays = await _arraySensorService.GetAllAsync();
            return View(arrays);
        }

        [HttpGet]
        public IActionResult CreateArraySensor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArraySensor(CreateArraySensorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _arraySensorService.CreateAsync(dto);
            return RedirectToAction("ArraySensors");
        }

        public async Task<IActionResult> DeleteArraySensor(int id)
        {
            var success = await _arraySensorService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("ArraySensors");
        }

        // ALERTAS

        public async Task<IActionResult> Alertas(string? gravidade)
        {
            var alertas = await _alertaService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(gravidade))
                alertas = alertas
                    .Where(a => a.Gravidade.Equals(gravidade, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return View("Alertas", alertas);
        }

        [HttpGet]
        public IActionResult CreateAlerta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlerta(CreateAlertaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _alertaService.CreateAsync(dto);
            return RedirectToAction("Alertas");
        }

        public async Task<IActionResult> DeleteAlerta(int id)
        {
            var success = await _alertaService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Alertas");
        }

        // JSON endpoints para atualizações em tempo real

        [HttpGet]
        public async Task<IActionResult> ObterSensoresJson()
        {
            var sensores = await _sensorService.GetAllAsync();
            return Json(sensores);
        }

        [HttpGet]
        public async Task<IActionResult> ObterAlertasJson(string? gravidade)
        {
            var alertas = await _alertaService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(gravidade))
                alertas = alertas
                    .Where(a => a.Gravidade.Equals(gravidade, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return Json(alertas
                .OrderByDescending(a => a.DataHoraColeta)
                .Select(a => new {
                    a.Id,
                    a.DataHoraColeta,
                    a.NomeLocal,
                    a.SensorId,
                    a.Gravidade,
                    a.ProbabilidadeIncendio,
                    a.Observacao
                }));
        }

        [HttpGet]
        public async Task<IActionResult> ObterArraysJson()
        {
            var arrays = await _arraySensorService.GetAllAsync();

            var result = arrays.Select(a => new
            {
                a.Id,
                Nome = a.NomeLocal,
                Sensores = a.Sensores.Select(s => new
                {
                    s.Id,
                    s.NomeLocal,
                    s.Temperatura,
                    s.NivelCO2,
                    s.DataUltimaLeitura
                })
            });

            return Json(result);
        }
    }
}
