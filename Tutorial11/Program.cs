using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.Middlewares;
using Tutorial11.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<ClinicDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
