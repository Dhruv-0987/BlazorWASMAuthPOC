namespace wasmwithids.Server;

public class OpenIdConnectOptions
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ResponseType { get; set; }
    public string ResponseMode { get; set; }
    public List<string> Scope { get; set; } = new List<string>();
    public string CallbackPath { get; set; }
    public bool MapInboundClaims { get; set; }
    public bool GetClaimsFromUserInfoEndpoint { get; set; }
    public bool SaveTokens { get; set; }
}
