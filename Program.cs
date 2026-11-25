using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CharacterHub API", Version = "v1" });
});

// Persistencia: archivo JSON (Singleton porque mantiene el lock del archivo).
builder.Services.AddSingleton<IPersonajeRepository, JsonPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
