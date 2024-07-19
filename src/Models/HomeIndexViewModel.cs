
using RTMProxy.Services;
using System.Runtime.CompilerServices;

namespace RTMProxy.Models
{
    public class HomeIndexViewModel(IProxyConfigService proxyConfigService)
    {
        public IProxyConfigService ProxyConfigService { get; } = proxyConfigService;

        ProxyConfig Config { get; set; }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            this.Config = await this.ProxyConfigService.GetAsync(cancellationToken);
        }



        public Task SaveAsync(CancellationToken cancellationToken = default) => this.ProxyConfigService.SaveAsync(Config, cancellationToken);
    }

}
