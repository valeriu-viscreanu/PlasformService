using Consul;
using Microsoft.Extensions.Options;

internal class ConsulServiceRegister : IHostedService, IConsulServiceRegister
{
    private readonly PlatformConfig platformCfg;
    private readonly ConsulConfig consulCfg;
    private readonly IConsulClient client;

    public ConsulServiceRegister(IConsulClient client,
    IOptions<PlatformConfig> platformCfg,
    IOptions<ConsulConfig> consulCfg)
    {
        this.consulCfg = consulCfg.Value;
        this.platformCfg = platformCfg.Value;
        this.client = client;
    }
public async Task StartAsync(CancellationToken cancellationToken)
{

    var platformUri = new Uri(platformCfg.Url);
    var agentServiceRegistration = new AgentServiceRegistration() 
    {
        Address = platformUri.Host,
        Port = platformUri.Port,
        Name = platformCfg.ServiceName,
        ID = platformCfg.ServiceId

    };
    
    //await client.Agent.ServiceDeregister(platformCfg.ServiceId, cancellationToken);
    await client.Agent.ServiceRegister(agentServiceRegistration, cancellationToken);    
}

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await client.Agent.ServiceDeregister(platformCfg.ServiceId, cancellationToken);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("deregister error: " + ex);
        }
    }
}