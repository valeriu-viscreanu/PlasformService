using Consul;
using Microsoft.Extensions.Options;

public class ConsulRegistryService : IConsulRegistryService
{
    private readonly ConsulConfig consulCfg;
    private readonly IConsulClient client;

    public ConsulRegistryService(IConsulClient client,
   IOptions<ConsulConfig> consulCfg)
    {
        this.consulCfg = consulCfg.Value;
        this.client = client;
    }


    public Uri GetServiceUri(string serviceName) //todo remove the default value
    {
        var consulUrl = consulCfg.Url;
        var serviceQueryResult = client.Health.Service(serviceName).Result;

        if(serviceQueryResult is not null &&
            serviceQueryResult.Response is not null &&
            serviceQueryResult.Response.Length > 0)
            {
                var service = serviceQueryResult.Response.First();    
                return new Uri(service.Service.Address + ":" + service.Service.Port);
            }


        return null;
    }
}