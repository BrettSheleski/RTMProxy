using Microsoft.AspNetCore.Mvc;
using RTMProxy.Models;
using System.Diagnostics;

namespace RTMProxy.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = this.HttpContext.RequestServices.GetService<HomeIndexViewModel>()
                ?? throw new InvalidProgramException();

            await vm.InitializeAsync(HttpContext.RequestAborted);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeIndexViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                await viewModel.SaveAsync(HttpContext.RequestAborted);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}
