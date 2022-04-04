using DockerAdmin.Interfaces;
using DockerAdmin.Models;
using DockerAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DockerAdmin.Controllers
{
    public class CleanupController : Controller
    {
        private readonly IDockerService _dockerService;

        public CleanupController(IDockerService dockerService)
        {
            _dockerService = dockerService;
        }

        public IActionResult Index()
        {
            return View(new PruneResultViewModel());
        }

        [Route("prune/{option}")]
        public IActionResult Prune(PruneOption option)
        {
            string result = _dockerService.Prune(option);
            return View("Index", new PruneResultViewModel(option, result));
        }
    }
}
