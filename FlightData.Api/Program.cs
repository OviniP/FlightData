using Serilog;
using FlightData.Services;
using FlightData.Services.Contracts;
using FlightData.Api.Middleware;
using FlightData.Entities;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog (use Console and File sinks)
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
// Bind ApiSettings and add it to the services collection
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDataReaderService, FileReaderService>();
builder.Services.AddSingleton<IFlightSequenceValidator, FlightSequenceValidator>();
builder.Services.AddSingleton<IFlightDataService,FlightDataService>();


var app = builder.Build();
app.UseExceptionHandler(); // This should come first
app.UseSerilogRequestLogging(); // Log after exception middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
