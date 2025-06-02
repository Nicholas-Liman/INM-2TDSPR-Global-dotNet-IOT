using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AshBoard.Service.Services;
using AshBoard.Application.Repositories;
using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Domain.Entities;

public class ArraySensorServiceTests
{
    [Fact]
    public async Task GetAllAsync_DeveRetornarArrays()
    {
        var mockArrayRepo = new Mock<IArraySensorRepository>();
        var mockSensorRepo = new Mock<ISensorRepository>();

        mockArrayRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ArraySensor> {
                new ArraySensor { Id = 1, NomeLocal = "A" }
            });

        var service = new ArraySensorService(mockArrayRepo.Object, mockSensorRepo.Object);
        var resultado = await service.GetAllAsync();

        Assert.Single(resultado);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarComSensoresValidos()
    {
        var mockArrayRepo = new Mock<IArraySensorRepository>();
        var mockSensorRepo = new Mock<ISensorRepository>();

        mockSensorRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Sensor> {
                new Sensor { Id = "S1" }
            });

        var service = new ArraySensorService(mockArrayRepo.Object, mockSensorRepo.Object);
        var dto = new CreateArraySensorDto { NomeLocal = "Z", SensorIds = new() { "S1" } };

        var result = await service.CreateAsync(dto);

        Assert.Equal("Z", result.NomeLocal);
    }
}
