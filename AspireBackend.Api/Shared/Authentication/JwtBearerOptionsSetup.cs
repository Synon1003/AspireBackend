using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspireBackend.Api.Shared.Authentication;

public class JwtBearerOptionsSetup(IOptions<AuthOptions> authOptions): IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name == JwtBearerDefaults.AuthenticationScheme || string.IsNullOrEmpty(name))
        {
            Configure(options);
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        options.Authority = authOptions.Value.Authority;
        //options.Audience = authOptions.Value.Audience;
        options.MapInboundClaims = false;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuers = [authOptions.Value.Issuer],
            ValidAudience = authOptions.Value.Audience.StartsWith("api://") ? 
                authOptions.Value.Audience : $"api://{authOptions.Value.Audience}"
        };
    }
}