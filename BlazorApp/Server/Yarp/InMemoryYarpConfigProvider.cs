using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace wasmwithids.Server.Yarp;

public class InMemoryYarpConfigProvider: IProxyConfigProvider
{
    private volatile InMemoryConfig _config;
    
    public InMemoryYarpConfigProvider(ProxyConfigOptions proxyConfigOptions)
    {
        _config = new InMemoryConfig(proxyConfigOptions.Routes, proxyConfigOptions.Clusters);
    }
    
    // interface method returning the config for the YARP setup as read only
    public IProxyConfig GetConfig()
    {
        return _config;
    }
    
    public void UpdateRoutes(IEnumerable<RouteConfig> routes)
    {
        var clusters = _config.Clusters;
        _config = new InMemoryConfig(routes, clusters);
    }

    public void UpdateClusters(IEnumerable<ClusterConfig> clusters)
    {
        var routes = _config.Routes;
        _config = new InMemoryConfig(routes, clusters);
    }

    private class InMemoryConfig : IProxyConfig
    {
        public InMemoryConfig(IEnumerable<RouteConfig> routes, IEnumerable<ClusterConfig> clusters)
        {
            Routes = routes.ToList().AsReadOnly();
            Clusters = clusters.ToList().AsReadOnly();
            ChangeToken = new CancellationChangeToken(new CancellationToken(false));
        }
        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken { get; }
    }
}