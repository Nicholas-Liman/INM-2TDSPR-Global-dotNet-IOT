using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Infrastructure.Repositories;
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

// Registro dos repositórios da aplicação
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IArraySensorRepository, ArraySensorRepository>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

// Suporte a Razor Views para o dashboard
builder.Services.AddControllersWithViews();

// Configuração do Swagger
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

// Habilita o uso de arquivos estáticos, como CSS e JS para as views Razor
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota padrão para o Dashboard com Razor Views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

// Mapeia os controllers da API REST
app.MapControllers();

app.Run();
