using CustomerService.Application.Common;
using CustomerService.Application.Handlers;
using CustomerService.Application.Middleware;
using CustomerService.Infrastructure.ExternalServices;
using CustomerService.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Auth Service client
builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri("https://localhost:44317"); // Auth Service URL
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerHandler).Assembly);
});


// Persistence katmaný
builder.Services.AddPersistence();

builder.Services.AddHttpClient<IKycService, KycService>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseMiddleware<ExternalAuthMiddleware>();

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
