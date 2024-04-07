using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace wasmwithids.Server.Yarp;

public class AccessTokenTransformProvider: ITransformProvider
{
    private readonly HttpContextAccessor _httpContextAccessor;
    
    public AccessTokenTransformProvider(HttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void ValidateRoute(TransformRouteValidationContext context)
    {
        
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
        
    }

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(async transformContext =>
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        });
    }
}