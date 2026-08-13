using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace GymQuest.Api.Configuration;

/// <summary>
/// Por defecto, AddOpenApi() no declara ningún esquema de seguridad en el
/// documento OpenAPI generado, así que Scalar (o cualquier UI) no tiene
/// forma de saber que esta API usa JWT y no dibuja ningún botón de
/// autenticación.
///
/// Este transformer le agrega al documento la definición del esquema
/// "Bearer" y se la aplica a TODOS los endpoints, para que en Scalar
/// aparezca el botón de "Authorize" donde puedes pegar el token.
///
/// Se registra en Program.cs con:
///   builder.Services.AddOpenApi(options =>
///       options.AddDocumentTransformer&lt;BearerSecuritySchemeTransformer&gt;());
/// </summary>
internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var esquemasDeAutenticacion = await authenticationSchemeProvider.GetAllSchemesAsync();

        // Solo agrega el esquema Bearer al documento si de verdad está
        // configurado en el pipeline (evita declarar algo que no existe).
        if (!esquemasDeAutenticacion.Any(esquema => esquema.Name == "Bearer"))
        {
            return;
        }

        var esquemaBearer = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Pega aquí el token JWT que devuelve /api/auth/login o /api/auth/registro."
        };

        document.Components ??= new OpenApiComponents();
        document.AddComponent("Bearer", esquemaBearer);

        var requisitoDeSeguridad = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        };

        foreach (var operacion in document.Paths.Values.SelectMany(path => path.Operations))
        {
            operacion.Value.Security ??= new List<OpenApiSecurityRequirement>();
            operacion.Value.Security.Add(requisitoDeSeguridad);
        }
    }
}