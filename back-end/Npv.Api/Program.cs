#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Npv.Api.Services;

var builder = WebApplication.CreateBuilder(args);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


builder.Services.AddScoped<INpvCalculatorService, NpvCalculatorService>();

builder.Services
.AddCors(o =>
    o.AddDefaultPolicy(b =>
    b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger services
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
