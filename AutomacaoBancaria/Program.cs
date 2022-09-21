using AutomacaoBancaria.Adapters.Sql.Repository;
using AutomacaoBancaria.Domain.Application.Services;
using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Interfaces.Application.Services;
using AutomacaoBancaria.Domain.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conexao = builder.Configuration.GetSection("ConnectionStrings").GetSection("mssql").Get<ConnectionStringSettings>();
builder.Services.AddSingleton(conexao);

builder.Services.AddScoped<ITitularServices, TitularServices>();
builder.Services.AddScoped<ITitularRepository, TitularRepository>();
builder.Services.AddScoped<IContaCorrenteServices, ContaCorrenteServices>();
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();