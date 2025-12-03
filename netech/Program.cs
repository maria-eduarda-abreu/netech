using Microsoft.EntityFrameworkCore;
using netech.Api.Middlewares;
using netech.Core.Interfaces;
using netech.Infrastructure.Data;
using netech.Infrastructure.Repositories;
using netech.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Banco de Dados (Infraestrutura)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Injeção de Dependência (DI) - Item 2.2 do Relatório
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ICarbonCalculatorService, CarbonCalculatorService>();

// 3. Configurar Exception Handler Global
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Necessário para RFC 7807

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Pipeline de Requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // Ativa o GlobalExceptionHandler
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();