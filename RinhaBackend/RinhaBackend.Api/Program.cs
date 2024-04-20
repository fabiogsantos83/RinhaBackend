using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RinhaBackend.Api.Workers;
using RinhaBackend.Application;
using RinhaBackend.Domain.Commands;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Interfaces;
using RinhaBackend.Domain.Validators;
using RinhaBackend.Infrastructure.Repositories;
using System;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using NATS.Client.Core;
using NATS.Client.Hosting;
using System.Collections.Concurrent;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPessoaRepository, PessoaRepository>();
builder.Services.AddTransient<IPessoaService, PessoaService>();
builder.Services.AddScoped<IValidator<AddPessoaRequest>, AddPessoaRequestValidator>();
builder.Services.AddSingleton(_ => new ConcurrentQueue<PessoaEntity>());

builder.Services.AddNpgsqlDataSource(
    Environment.GetEnvironmentVariable(
        "DB_CONNECTION_STRING") ??
        "ERRO de connection string!!!");

//builder.Services.AddHostedService<AddPessoaDBWorker>();
builder.Services.AddNats(1, configureOpts: options => NatsOpts.Default with { Url = Environment.GetEnvironmentVariable("NATS_URL") });

//var natsDestination = Environment.GetEnvironmentVariable("NATS_DESTINATION");
//var natsOwnChannel = Environment.GetEnvironmentVariable("NATS_OWN");

//builder.Services.AddSingleton<string>(natsOwnChannel ?? "");

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is ValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("");
        }
    });
});


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
