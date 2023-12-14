using API_TCC.Controllers;
using API_TCC.Database;
using API_TCC.Repositories;
using API_TCC.Repository;
using API_TCC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
//builder.Services.AddSingleton<OracleConnection>(_ => new OracleConnection(builder.Configuration.GetConnectionString("OracleConnection")));
builder.Services.AddScoped<MonitoramentoService, MonitoramentoService>();
//builder.Services.AddScoped<MonitoramentoController>();
builder.Services.AddScoped<PlantasService, PlantasService>();
//builder.Services.AddSingleton<IMonitoramentoRepository>();
//builder.Services.AddHostedService<MeuServicoMqtt>();
builder.Services.AddSingleton<MeuServicoMqtt>();
builder.Services.AddSingleton<OracleConnection>();
builder.Services.AddSingleton<IPlantasRepository, PlantasService>();

builder.Services.AddSingleton<IMonitoramentoRepository, MonitoramentoService>();
builder.Services.AddSingleton<IUsuarioRepository, UsuarioService>();
builder.Services.AddScoped<UsuarioService, UsuarioService>();
builder.Services.AddSingleton<IServiceEnvioMqtt, ServiceEnvioMqtt>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TCC SMARTGREEN", Version = "v1" });
});

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

//builder.Services.AddDbContext<MyDbContext>(options => options.UseOracle(config.GetConnectionString("OracleConnection")));

builder.Services.AddDbContext<MyDbContext>(options => options.UseOracle(config.GetConnectionString("OracleConnection")), ServiceLifetime.Singleton);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configuração de pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TCC");
});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.Run();
