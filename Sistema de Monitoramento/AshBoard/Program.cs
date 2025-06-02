using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Data.Repositories;
using AshBoard.Service.Services;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados Oracle
builder.Services.AddDbContext<AshBoardDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IArraySensorService, ArraySensorService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<ILeituraService, LeituraService>();
builder.Services.AddScoped<IAlertaMLService, AlertaMLService>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IArraySensorRepository, ArraySensorRepository>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

builder.Services.AddControllersWithViews();

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

    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"]! });
    c.DocInclusionPredicate((docName, apiDesc) => true);
});

var app = builder.Build();

// Middleware do Swagger — acessível apenas em desenvolvimento via /swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AshBoard API v1");
        c.RoutePrefix = "api-docs";  // Configuração do RoutePrefix para não interferir na raiz
    });
}

// Middlewares da aplicação
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Rota padrão: MVC View → Dashboard/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
