using Yarp.ReverseProxy.Configuration;

namespace wasmwithids.Server.Yarp;

public class ProxyConfigOptions
{
    public List<RouteConfig> Routes { get; set; }
    public List<ClusterConfig> Clusters { get; set; }
}