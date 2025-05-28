using Xunit;
using Moq;
using AshBoard.Infrastructure.Services;
using AshBoard.Application.Interfaces;
using AshBoard.Domain.Entities;
using AshBoard.Application.DTOs.Alerta;
using System.Threading.Tasks;

public class AlertaServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldGenerateAlerta()
    {
        // Arrange
        var mockRepo = new Mock<IAlertaRepository>();
        var alertaService = new AlertaService(mockRepo.Object);

        var dto = new CreateAlertaDto
        {
            Nivel = "vermelho",
            Mensagem = "Incêndio detectado"
        };

        // Act
        await alertaService.CreateAsync(dto);

        // Assert
        mockRepo.Verify(r => r.CreateAsync(It.Is<AlertaEntity>(
            a => a.Nivel == "vermelho" && a.Mensagem.Contains("Incêndio"))), Times.Once);
    }
}
