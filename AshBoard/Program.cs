using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Data.Repositories;
using AshBoard.Service.Services;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AshBoardDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AshBoard API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
