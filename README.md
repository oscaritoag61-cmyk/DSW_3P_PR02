# CharacterHub API (DSW_3P_PR02)

API REST en ASP.NET Core 8 para administrar personajes de videojuegos.
Arquitectura MVC + Service + Repository, persistencia en SQL Server (EF Core), autenticación JWT, CORS y Swagger UI.

## Estructura

| Capa | Archivos |
|------|----------|
| Modelo | `Models/Personaje.cs` |
| DTOs y validación | `DTOs/`, `Validation/ImagenUrlAttribute.cs` |
| Controller | `Controllers/PersonajesController.cs`, `Controllers/AuthController.cs` |
| Service | `Services/PersonajeService.cs`, `Services/AuthService.cs` |
| Repository | `Repositories/IPersonajeRepository.cs`, `JsonPersonajeRepository.cs` (etapa 1), `SqlPersonajeRepository.cs` (etapa 2) |
| EF Core | `Data/CharacterHubContext.cs`, `Migrations/` |

## Requisitos

- .NET SDK 8
- SQL Server (por defecto `.\SQLEXPRESS` con autenticación de Windows)
- Herramienta EF: `dotnet tool install --global dotnet-ef --version 8.*`

## Configuración de secretos (no están en el repositorio)

La clave de firma JWT y la contraseña del usuario demo se guardan con *user-secrets*:

```powershell
dotnet user-secrets set "Jwt:Key" "<clave aleatoria de al menos 32 caracteres>"
dotnet user-secrets set "DemoUser:Password" "<contraseña del usuario demo>"
```

También pueden darse como variables de entorno: `Jwt__Key` y `DemoUser__Password`.
Sin `Jwt:Key` la aplicación no arranca y muestra un mensaje indicando cómo configurarla.

## Base de datos

```powershell
dotnet ef database update
```

Crea la base `CharacterHubDb` y la tabla `Personajes` con 3 registros iniciales.
La cadena de conexión está en `appsettings.json` → `ConnectionStrings:CharacterHub`.

## Ejecutar

```powershell
dotnet run --launch-profile http
```

- URL base: `http://localhost:5184`
- Swagger UI: `http://localhost:5184/swagger`

## Endpoints

| Método | Endpoint | Auth | Respuestas |
|--------|----------|------|------------|
| POST | `/api/auth/login` | No | 200 token · 400 · 401 |
| GET | `/api/personajes` | Sí | 200 |
| GET | `/api/personajes/{id}` | Sí | 200 · 404 |
| GET | `/api/personajes/videojuego/{nombre}` | Sí | 200 |
| GET | `/api/personajes/nivel/{nivel}` | Sí | 200 · 400 |
| POST | `/api/personajes` | Sí | 201 · 400 |
| PUT | `/api/personajes/{id}` | Sí | 200 · 400 · 404 |
| DELETE | `/api/personajes/{id}` | Sí | 204 · 404 |

Sin token o con token inválido/expirado se responde **401**.

### Login

```http
POST /api/auth/login
Content-Type: application/json

{ "username": "admin", "password": "<contraseña demo>" }
```

Respuesta:

```json
{ "token": "eyJ...", "tokenType": "Bearer", "expiration": "2026-...Z", "expiresIn": 3600, "username": "admin" }
```

Usar el token en cada petición: `Authorization: Bearer <token>`.
La expiración se configura en `Jwt:ExpirationMinutes` (60 por defecto).

### Registrar personaje

```json
{
  "nombre": "Lara Croft",
  "videojuego": "Tomb Raider",
  "clase": "Exploradora",
  "nivel": 50,
  "poder": 6000,
  "imagenUrl": "https://placehold.co/400x400/png?text=Lara"
}
```

`disponible` no se envía al registrar: siempre queda en `true`. En `PUT` sí se puede enviar.

Reglas: nombre, videojuego y clase obligatorios · nivel 1–100 · poder > 0 · `imagenUrl` absoluta http/https.

## CORS

Los orígenes permitidos están en `appsettings.json`:

```json
"Cors": { "AllowedOrigins": [ "http://127.0.0.1:5500", "http://localhost:5500" ] }
```

Para el cliente del docente basta agregar su origen (esquema + host + puerto, sin `/` final) y reiniciar la API.
Alternativa sin editar archivos: `$env:Cors__AllowedOrigins__0 = "http://origen-del-docente:puerto"`.

## Cambio de persistencia JSON → SQL Server

Archivos agregados o modificados:

- `Data/CharacterHubContext.cs` (nuevo): DbContext, configuración de columnas y datos iniciales.
- `Repositories/SqlPersonajeRepository.cs` (nuevo): implementa `IPersonajeRepository` con EF Core.
- `Migrations/` (nuevo): migración `InitialCreate`.
- `Program.cs`: se registra `AddDbContext` y se cambia `JsonPersonajeRepository` por `SqlPersonajeRepository` en la inyección de dependencias.
- `appsettings.json`: cadena de conexión.
- `DSW_3P_PR02.csproj`: paquetes de EF Core.

Sin cambios: `PersonajesController`, `PersonajeService` e `IPersonajeRepository`.
