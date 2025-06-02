using Xunit;
using Moq;
using System.Threading.Tasks;
using AshBoard.Service.Services;
using AshBoard.Application.Repositories;
using AshBoard.Application.Interfaces;
using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.Leitura;
using AshBoard.Domain.Entities;

public class SensorServiceTests
{
    [Fact]
    public async Task DeveCriarSensor()
    {
        var repo = new Mock<ISensorRepository>();
        var ml = new Mock<IAlertaMLService>();
        var service = new SensorService(repo.Object, ml.Object);

        var dto = new CreateSensorDto { Id = "S1", NomeLocal = "X" };
        var result = await service.CreateAsync(dto);

        Assert.Equal("S1", result.Id);
    }

    [Fact]
    public async Task DeveAtualizarLeitura()
    {
        var repo = new Mock<ISensorRepository>();
        var ml = new Mock<IAlertaMLService>();
        var sensor = new Sensor { Id = "S1" };

        repo.Setup(r => r.GetByIdAsync("S1")).ReturnsAsync(sensor);
        ml.Setup(m => m.ObterProbabilidadeIncendio(30, 900)).Returns(55);

        var service = new SensorService(repo.Object, ml.Object);
        var leitura = new LeituraSensorDto { SensorId = "S1", Temperatura = 30, NivelCO2 = 900 };

        var ok = await service.AtualizarLeituraAsync("S1", leitura);

        Assert.True(ok);
        Assert.Equal(30, sensor.Temperatura);
    }
}
