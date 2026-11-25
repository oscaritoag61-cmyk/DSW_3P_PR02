using System.Text;
using DSW_3P_PR02.Auth;
using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CharacterHub API", Version = "v1" });

    // Botón "Authorize" en Swagger UI para enviar el JWT.
    var esquema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Pega aquí el token obtenido en POST /api/auth/login (sin la palabra Bearer).",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", esquema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { esquema, Array.Empty<string>() } });
});

// Persistencia: SQL Server con Entity Framework Core.
// (Versión anterior: builder.Services.AddSingleton<IPersonajeRepository, JsonPersonajeRepository>();)
builder.Services.AddDbContext<CharacterHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CharacterHub")));
builder.Services.AddScoped<IPersonajeRepository, SqlPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

// ---------- Autenticación JWT ----------
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Section));
builder.Services.Configure<DemoUserSettings>(builder.Configuration.GetSection(DemoUserSettings.Section));
builder.Services.AddScoped<IAuthService, AuthService>();

var jwt = builder.Configuration.GetSection(JwtSettings.Section).Get<JwtSettings>() ?? new JwtSettings();
if (string.IsNullOrWhiteSpace(jwt.Key) || Encoding.UTF8.GetByteCount(jwt.Key) < 32)
{
    throw new InvalidOperationException(
        "Falta la clave JWT (mínimo 32 caracteres). Configúrala fuera del repositorio con:\n" +
        "  dotnet user-secrets set \"Jwt:Key\" \"<clave>\"\n" +
        "o con la variable de entorno Jwt__Key.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
