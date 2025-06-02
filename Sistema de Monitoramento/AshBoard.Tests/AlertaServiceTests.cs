using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AshBoard.Service.Services;
using AshBoard.Data.AppData;
using AshBoard.Application.DTOs.Alerta;
using AshBoard.Application.Interfaces;

public class AlertaServiceTests
{
    [Fact]
    public async Task CreateAsync_ComGravidadeAmarelo_AdicionaObservacao()
    {
        var options = new DbContextOptionsBuilder<AshBoardDbContext>()
            .UseInMemoryDatabase("AlertasDb")
            .Options;

        var context = new AshBoardDbContext(options);
        var mlMock = new Mock<IAlertaMLService>();
        mlMock.Setup(m => m.ObterProbabilidadeIncendio(It.IsAny<float>(), It.IsAny<float>()))
              .Returns(70f);

        var service = new AlertaService(context, mlMock.Object);

        var dto = new CreateAlertaDto
        {
            SensorId = "1",
            NomeLocal = "Simulado",
            Gravidade = "Amarelo",
            DataHoraColeta = DateTime.UtcNow,
            Temperatura = 42,
            NivelCO2 = 1100,
            Latitude = 0,
            Longitude = 0
        };

        var result = await service.CreateAsync(dto);

        Assert.Contains("70", result.Observacao);
    }
}
