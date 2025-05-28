using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AshBoardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ou UseInMemoryDatabase("AshBoard")

// Registro dos serviços da aplicação
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IArraySensorService, ArraySensorService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();

// Configuração dos controllers
builder.Services.AddControllers();

// Configuração do Swagger com suporte a anotações
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AshBoard API",
        Version = "v1",
        Description = "API para monitoramento de sensores e alertas de incêndio"
    });
});

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AshBoard API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
