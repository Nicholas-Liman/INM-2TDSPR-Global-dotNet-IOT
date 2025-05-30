using Xunit;
using Moq;
using AshBoard.Application.Interfaces;
using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.Repositories;
using AshBoard.Domain.Entities;
using AshBoard.Service.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SensorServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnSensorsList()
    {
        // Arrange
        var mockRepo = new Mock<ISensorRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Sensor>
        {
            new Sensor { Id = "abc123", NomeLocal = "Sensor 1" },
            new Sensor { Id = "def456", NomeLocal = "Sensor 2" }
        });

        var service = new SensorService(mockRepo.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.NomeLocal == "Sensor 1");
        Assert.Contains(result, s => s.NomeLocal == "Sensor 2");
    }
}
