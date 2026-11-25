using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CharacterHub API", Version = "v1" });
});

// Persistencia: SQL Server con Entity Framework Core.
// (Versión anterior: builder.Services.AddSingleton<IPersonajeRepository, JsonPersonajeRepository>();)
builder.Services.AddDbContext<CharacterHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CharacterHub")));
builder.Services.AddScoped<IPersonajeRepository, SqlPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
