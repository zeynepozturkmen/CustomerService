using CustomerService.Application.Common;
using CustomerService.Application.Handlers;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.ExternalServices;
using CustomerService.Persistence;
using CustomerService.Persistence.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerHandler).Assembly);
});


// Persistence katmaný
builder.Services.AddPersistence();

builder.Services.AddHttpClient<IKycService, KycService>();



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
