using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Middlewares;
using Tutorial11.Services;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPatientService, PatientService>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddDbContext<ClinicDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ClinicDbContext>();
    await dbContext.Database.MigrateAsync();
    await DbInitializer.SeedData(dbContext);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opt.RoutePrefix = string.Empty;
    });
}
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
