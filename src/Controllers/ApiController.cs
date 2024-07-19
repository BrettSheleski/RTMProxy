using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RTMProxy.Services;

namespace RTMProxy.Controllers
{
    [ApiController]
    public class ApiController(IProxyConfigService proxyConfigService, INginxService nginxService) : ControllerBase
    {
        public IProxyConfigService ProxyConfigService { get; } = proxyConfigService;
        public INginxService NginxService { get; } = nginxService;

        [HttpPost]
        [Route("/api")]
        public async Task<ActionResult> SetConfig(ProxyConfig config)
        {
            CancellationToken cancellationToken = HttpContext.RequestAborted;

            await ProxyConfigService.SaveAsync(config, cancellationToken);

            await NginxService.RestartAsync(cancellationToken);

            return Ok();
        }
    }
}
