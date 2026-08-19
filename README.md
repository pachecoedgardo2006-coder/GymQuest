# 🎮 GymQuest — RPG Fitness

> Transforma tu rutina de gimnasio en un videojuego de rol. Tu progreso real en fuerza y disciplina se refleja en el nivel, atributos y racha de tu héroe.

**Estado actual:** ✅ Backend V1 funcional de punta a punta (probado manualmente en Scalar) · ⬜ Siguiente fase: Frontend Móvil (.NET MAUI)

---

## 📖 Tabla de contenido

- [Visión del proyecto](#-visión-del-proyecto)
- [Mecánicas principales](#-mecánicas-principales)
- [Arquitectura](#-arquitectura)
- [Stack tecnológico](#-stack-tecnológico)
- [Estructura del repositorio](#-estructura-del-repositorio)
- [Puesta en marcha](#-puesta-en-marcha)
- [Variables de entorno](#-variables-de-entorno)
- [Endpoints de la API](#-endpoints-de-la-api)
- [Testing](#-testing)
- [Roadmap](#-roadmap)
- [Límites de la V1](#-límites-establecidos-para-la-v1)

---

## 🎯 Visión del proyecto

GymQuest es una aplicación móvil que gamifica el entrenamiento de fuerza. El usuario administra un "héroe" cuyo progreso (nivel, XP, atributos) es un reflejo directo de su disciplina y mejora física real, eliminando la monotonía del registro tradicional de ejercicios.

## ⚔️ Mecánicas principales

- **XP y niveles**: subir peso o repeticiones otorga XP ("Boost de Sobrecarga"). Completar una rutina otorga XP base. Subir de nivel desbloquea marcos de perfil y títulos.
- **Rachas ("Racha de Hierro")**: cumplir los días de entrenamiento programados mantiene la racha activa y otorga multiplicadores de XP y "Pociones de Descanso".
- **Atributos del héroe**: Fuerza (STR), Resistencia (END) y Consistencia (CON), cada uno alimentado por distintos tipos de entrenamiento.
- **Logros y misiones**: diarias/semanales, con progreso medible (p. ej. "Aumenta 2.5 kg en cualquier ejercicio hoy").

## 🏛️ Arquitectura

El backend sigue **Clean Architecture** estricta, con las dependencias apuntando siempre hacia el `Domain`:

```
Domain            ← no depende de nada (reglas de negocio puras)
   ↑
Application       ← depende de Domain (casos de uso)
   ↑
Infrastructure    ← depende de Application y Domain (EF Core, JWT, repos)
   ↑
Api               ← depende de Infrastructure y Application (controllers, HTTP)
```

**Regla de oro:** `Domain` nunca sabe que existen `Application`, `Infrastructure` o `Api`.

## 🛠️ Stack tecnológico

### Backend

| Categoría | Tecnología |
|---|---|
| Lenguaje | C# (.NET 10) |
| Framework web | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Base de datos | PostgreSQL (`gymquest_db`) |
| Driver de BD | Npgsql.EntityFrameworkCore.PostgreSQL |
| Administración de BD | pgAdmin4 |
| Autenticación | JWT (JSON Web Tokens) — `System.IdentityModel.Tokens.Jwt`, `Microsoft.IdentityModel.Tokens` |
| Hasheo de contraseñas | `Microsoft.Extensions.Identity.Core` (`IPasswordHasher<Usuario>`) |
| Validación de entrada | FluentValidation (v11.x) |
| Documentación de API | Scalar (`Scalar.AspNetCore`) sobre OpenAPI nativo — sin Swashbuckle/Swagger |
| Manejo de errores | `IExceptionHandler` (patrón .NET 8+) + `ProblemDetails` (RFC 7807) |
| Testing | xUnit, Moq, FluentAssertions |
| IDE | JetBrains Rider |
| Control de versiones | Git |

### Frontend móvil (próxima fase)

| Categoría | Tecnología |
|---|---|
| Framework | .NET MAUI (multiplataforma iOS/Android) |
| Patrón arquitectónico | MVVM |
| Cliente HTTP | `HttpClient` contra la Web API |

### Patrones y principios aplicados

- Clean Architecture (separación estricta por capas)
- Repository Pattern (interfaces en `Domain`, implementaciones en `Infrastructure`)
- Dependency Injection (contenedor nativo de ASP.NET Core)
- DTOs + mapeo manual (sin AutoMapper, decisión YAGNI)
- Principio de responsabilidad única, PascalCase/camelCase (Clean Code)

## 📁 Estructura del repositorio

```
GymQuest/
├── GymQuest.slnx
├── .gitignore
├── src/
│   ├── GymQuest.Domain/           # Entidades, enums, interfaces de repositorio
│   ├── GymQuest.Application/      # DTOs, servicios (casos de uso), validators
│   ├── GymQuest.Infrastructure/   # EF Core, repositorios concretos, JWT
│   └── GymQuest.Api/              # Controllers, Program.cs, appsettings
└── tests/
    ├── GymQuest.Domain.Tests/
    └── GymQuest.Application.Tests/
```

> Ver `GymQuest_Estructura_Proyecto.txt` para el árbol completo y detallado archivo por archivo.

## 🚀 Puesta en marcha

### Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local o remoto) + pgAdmin4 (opcional, para administración visual)
- JetBrains Rider (o cualquier IDE compatible con .NET)

### Pasos

```bash
# 1. Clonar el repositorio
git clone <url-del-repo>
cd GymQuest

# 2. Restaurar dependencias
dotnet restore

# 3. Configurar variables de entorno (ver sección siguiente)

# 4. Aplicar migraciones de EF Core
dotnet ef database update --project src/GymQuest.Infrastructure --startup-project src/GymQuest.Api

# 5. Ejecutar la API
dotnet run --project src/GymQuest.Api
```

La documentación interactiva (Scalar) estará disponible en `/scalar/v1` en modo desarrollo.

## 🔐 Variables de entorno

El backend necesita dos grupos de configuración sensible: **conexión a PostgreSQL** y **parámetros JWT**.

```env
ConnectionStrings__DefaultConnection=Host=localhost;Database=gymquest_db;Username=postgres;Password=tu_password
Jwt__Key=clave-secreta-de-al-menos-32-caracteres
Jwt__Issuer=GymQuest.Api
Jwt__Audience=GymQuest.Client
Jwt__ExpiracionMinutos=60
```

- ASP.NET Core lee variables de entorno de forma nativa (usa `__` en vez de `:`), no requiere ningún paquete adicional.
- Si prefieres un archivo `.env` estilo Node.js, instala `DotNetEnv` y colócalo en `src/GymQuest.Api/.env` (agrégalo a `.gitignore`).
- Nunca subas `appsettings.Development.json` con credenciales reales a git; mantenlo solo como plantilla local.

## 🌐 Endpoints de la API

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| POST | `/api/auth/registro` | Público | Registra usuario + crea su héroe automáticamente |
| POST | `/api/auth/login` | Público | Login, devuelve usuario + token JWT |
| GET | `/api/heroe/usuario/{usuarioId}/estadisticas` | 🔒 | Estadísticas del héroe (nivel, XP, atributos) |
| POST | `/api/rutina` | 🔒 | Crea una rutina personalizada |
| POST | `/api/entrenamiento/iniciar` | 🔒 | Inicia una sesión de entrenamiento |
| POST | `/api/entrenamiento/serie` | 🔒 | Registra una serie realizada en vivo |
| POST | `/api/entrenamiento/{sesionId}/finalizar` | 🔒 | Finaliza la sesión, calcula XP y detecta subida de nivel |
| GET | `/api/racha/heroe/{heroeId}` | 🔒 | Consulta la racha activa |
| POST | `/api/racha/heroe/{heroeId}/entrenamiento-hoy` | 🔒 | Registra el entrenamiento del día |
| GET | `/api/logro/heroe/{heroeId}` | 🔒 | Lista logros del héroe |
| POST | `/api/logro/{logroId}/desbloquear` | 🔒 | Desbloquea un logro |
| GET | `/api/mision/heroe/{heroeId}/activas` | 🔒 | Lista misiones activas |
| POST | `/api/mision/{misionId}/progreso` | 🔒 | Registra progreso de una misión |

🔒 = requiere header `Authorization: Bearer <token>`

## 🧪 Testing

```bash
dotnet test
```

- **39/39 pruebas en verde** en toda la solución
- 21 pruebas en `GymQuest.Domain.Tests` (lógica de negocio pura)
- 18 pruebas en `GymQuest.Application.Tests` (servicios, con Moq para repositorios)

## 🗺️ Roadmap

- [x] **Fase 1** — Solución, proyectos y referencias entre capas
- [x] **Fase 2.1** — Domain (10 entidades, 3 enums, 9 interfaces de repositorio)
- [x] **Fase 2.2** — Application (7 servicios, 16 DTOs, FluentValidation)
- [x] **Fase 2.3** — Infrastructure (EF Core + PostgreSQL, 8 repositorios, JWT)
- [x] **Fase 2.4** — Api (7 controllers, Scalar, manejo global de errores, CORS) — **backend V1 completo**
- [ ] **Fase 3** — Frontend Móvil (.NET MAUI, arquitectura MVVM)

### Backlog conocido (no bloqueante, revisar antes de producción)

- Códigos HTTP poco precisos: al no existir excepciones de dominio propias, casos que deberían ser 401/404/409 hoy devuelven 400.
- Ningún endpoint valida que el recurso solicitado pertenezca al usuario dueño del token.
- La creación de `Usuario` + `Heroe` en el registro no es atómica (sin Unit of Work).

### Visión a futuro (fuera de alcance de la V1)

- Clanes/Guilds y Boss Raids grupales
- Arena PvP
- Tienda cosmética con monedas virtuales
- Integración con wearables (Apple Health, Google Fit)
- Entrenador virtual con IA / rutinas adaptativas

## 🚧 Límites establecidos para la V1

Para mantener el proyecto ágil y enfocado, la V1 **no incluye**:

1. Funciones sociales/multijugador (sin amigos, chats ni leaderboards)
2. Tienda de microtransacciones ni economía virtual
3. Animación 3D del avatar (ilustración 2D tipo tarjeta RPG)
4. Integración con wearables/relojes inteligentes (ingreso de datos manual)
5. Algoritmos de IA / entrenador virtual adaptativo

---

*Desarrollado aplicando Clean Architecture y principios de Clean Code sobre .NET 10.*
