using Microsoft.AspNetCore.Mvc;
using RTMProxy.Models;
using RTMProxy.Services;
using System.Diagnostics;

namespace RTMProxy.Controllers
{
    public class HomeController(IProxyConfigService proxyConfigService, INginxService nginxService) : Controller
    {
        public IProxyConfigService ProxyConfigService { get; } = proxyConfigService;
        public INginxService NginxService { get; } = nginxService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new HomeIndexViewModel();

            vm.Config = await this.ProxyConfigService.GetAsync(this.HttpContext.RequestAborted);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeIndexViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                CancellationToken cancellationToken = HttpContext.RequestAborted;

                await this.ProxyConfigService.SaveAsync(viewModel.Config, cancellationToken);

                await this.NginxService.RestartAsync(cancellationToken);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}
