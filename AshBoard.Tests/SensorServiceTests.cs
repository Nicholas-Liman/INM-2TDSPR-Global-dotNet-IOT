using Xunit;
using Moq;
using AshBoard.Application.Interfaces;
using AshBoard.Application.DTOs.Sensor;
using AshBoard.Infrastructure.Services;
using AshBoard.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SensorServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnSensorsList()
    {
        // Arrange
        var mockRepo = new Mock<ISensorRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<SensorEntity>
        {
            new SensorEntity { Id = 1, Nome = "Sensor 1" },
            new SensorEntity { Id = 2, Nome = "Sensor 2" }
        });

        var service = new SensorService(mockRepo.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
