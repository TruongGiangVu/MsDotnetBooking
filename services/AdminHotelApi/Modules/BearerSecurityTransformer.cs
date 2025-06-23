using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace AdminHotelApi.Modules;

/// <summary>
/// Một triển khai <see cref="IOpenApiDocumentTransformer"/> dùng để thêm security scheme Bearer JWT
/// vào tài liệu OpenAPI và áp dụng nó như một yêu cầu bảo mật cho tất cả các endpoint.
/// </summary>
public sealed class BearerSecurityTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument doc, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        doc.Components ??= new();
        doc.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        // Định nghĩa bearer security scheme
        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Nhập 'Bearer {token}'",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        doc.Components.SecuritySchemes["Bearer"] = securityScheme;

        // Áp dụng bảo mật cho tất cả endpoint
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [securityScheme] = new List<string>()
        };

        foreach (var path in doc.Paths.Values)
        {
            foreach (var op in path.Operations.Values)
            {
                op.Security ??= new List<OpenApiSecurityRequirement>();
                op.Security.Add(securityRequirement);
            }
        }

        return Task.CompletedTask;
    }
}
