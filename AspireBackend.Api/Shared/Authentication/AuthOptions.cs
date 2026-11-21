using System.ComponentModel.DataAnnotations;

namespace AspireBackend.Api.Shared.Authentication;

public class AuthOptions
{
    public const string SectionName = "Auth";

    [Required]
    public string Authority {get; set;} = string.Empty;

    [Required]
    public string Audience {get; set;} = string.Empty;

    [Required]
    public string Issuer {get; set;} = string.Empty;

    [Required]
    public List<string> ApiScopes {get; set;} = [];
}