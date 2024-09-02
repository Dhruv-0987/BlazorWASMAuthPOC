using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Models;

public class AuthSignalOptions
{
    [Required]
    public string? BaseAddress { get; set; }
    
    [Required]
    public string? Secret { get; set; }
}