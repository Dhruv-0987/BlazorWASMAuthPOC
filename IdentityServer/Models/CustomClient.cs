using Duende.IdentityServer.Models;

namespace IdentityServerAspNetIdentity.Models;

public class CustomClient: Client
{
    public bool Requires2Fa { get; set; }
}