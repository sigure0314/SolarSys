using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Persistence;
using SolarMonitor.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "YourConnectionString";
builder.Services.AddDbContext<SolarDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<SiteService>();
builder.Services.AddScoped<InverterService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<AlarmService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

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
