using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Repositories;
using AshBoard.Application.Interfaces;
using AshBoard.Service.Services;
using AshBoard.Domain.Entities;
using System.Linq;

namespace AshBoard.Tests.Services
{
    public class ArraySensorServiceTests
    {
        private readonly Mock<IArraySensorRepository> _arrayRepoMock = new();
        private readonly Mock<ISensorRepository> _sensorRepoMock = new();

        [Fact]
        public async Task GetAllAsync_ShouldReturnArraySensorDtos()
        {
            // Arrange
            var arrays = new List<ArraySensor>
            {
                new ArraySensor
                {
                    Id = 1,
                    NomeLocal = "Bloco A",
                    Sensores = new List<Sensor>
                    {
                        new Sensor { Id = "S01" },
                        new Sensor { Id = "S02" }
                    }
                },
                new ArraySensor
                {
                    Id = 2,
                    NomeLocal = "Bloco B",
                    Sensores = new List<Sensor>()
                }
            };

            _arrayRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(arrays);

            var service = new ArraySensorService(_arrayRepoMock.Object, _sensorRepoMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var array1 = result.First();
            Assert.Equal(1, array1.Id);
            Assert.Equal("Bloco A", array1.NomeLocal);
            Assert.Equal(2, array1.Sensores.Count);
            Assert.Contains(array1.Sensores, s => s.Id == "S01");
        }
    }
}
