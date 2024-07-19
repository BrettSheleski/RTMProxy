namespace RTMProxy.Services
{
    public interface IProxyConfigService
    {
        Task<ProxyConfig> GetAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(ProxyConfig config, CancellationToken cancellationToken = default);
    }
}
