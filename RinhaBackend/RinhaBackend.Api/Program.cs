using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RinhaBackend.Application;
using RinhaBackend.Domain.Commands;
using RinhaBackend.Domain.Interfaces;
using RinhaBackend.Domain.Validators;
using RinhaBackend.Infrastructure.Repositories;
using System;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPessoaRepository, PessoaRepository>();
builder.Services.AddTransient<IPessoaService, PessoaService>();
builder.Services.AddScoped<IValidator<AddPessoaRequest>, AddPessoaRequestValidator>();

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
