using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using AspireBackend.Api.Shared.Authentication;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace AspireBackend.Api.Shared.OpenApi;

public static class OpenApiExtensions
{
    public static WebApplication UseScalar(this WebApplication app)
    {
        app.MapOpenApi();

        var authOptions = app.Services.GetRequiredService<IOptions<AuthOptions>>().Value;
        var scalarClientId = app.Configuration["SCALAR_CLIENTID"];

        if (string.IsNullOrWhiteSpace(scalarClientId))
        {
            throw new InvalidOperationException("SCALAR_CLIENTID is not configured");
        }

        if (authOptions.ApiScopes.Count == 0)
        {
            throw new InvalidOperationException("At least one API scope must be configured");
        }

        app.MapScalarApiReference(options =>
        {
            options.Theme = ScalarTheme.Moon;
            options.WithTitle("Scalar Api Docs");
            options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);

            options.AddDefaultScopes("OAuth2", authOptions.ApiScopes);
            options.AddPreferredSecuritySchemes("OAuth2")
                .AddAuthorizationCodeFlow("OAuth2", flow =>
                {
                    flow.ClientId = scalarClientId;
                    // flow.ClientSecret = "";
                    flow.Pkce = Pkce.Sha256;
                    flow.SelectedScopes = authOptions.ApiScopes;
                    flow.AuthorizationUrl = app.Configuration["SCALAR_AUTHORIZATIONURL"];
                    flow.TokenUrl = app.Configuration["SCALAR_TOKENURL"];
                    flow.RefreshUrl = app.Configuration["SCALAR_TOKENURL"];
                });
        });

        return app;
    }

    public static WebApplicationBuilder AddScalarApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
            options.AddDocumentTransformer((document, context, _) =>
            {
                var authOptions = context.ApplicationServices
                    .GetRequiredService<IOptions<AuthOptions>>()
                    .Value;

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["OAuth2"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(builder.Configuration["SCALAR_AUTHORIZATIONURL"]!),
                            TokenUrl = new Uri(builder.Configuration["SCALAR_TOKENURL"]!),
                            RefreshUrl = new Uri(builder.Configuration["SCALAR_TOKENURL"]!),
                            Scopes = authOptions.ApiScopes.ToDictionary(scope => scope, scope => scope)
                        }
                    }
                };

                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("OAuth2", document)] = [.. authOptions.ApiScopes]
                });

                return Task.CompletedTask;
            }));

        return builder;
    }
}