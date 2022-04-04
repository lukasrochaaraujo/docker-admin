using DockerAdmin.Interfaces;
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

        [Route("images")]
        public IActionResult PruneImages()
        {
            return View("Index", new PruneResultViewModel
            {
                Images = _dockerService.Prune(Models.PruneOption.Image)
            });
        }

        [Route("containers")]
        public IActionResult PruneContainers()
        {
            return View("Index", new PruneResultViewModel
            {
                Containers = _dockerService.Prune(Models.PruneOption.Container)
            });
        }

        [Route("volumes")]
        public IActionResult PruneVolumes()
        {
            return View("Index", new PruneResultViewModel
            {
                Volumes = _dockerService.Prune(Models.PruneOption.Volume)
            });
        }

        [Route("networks")]
        public IActionResult PruneNetworks()
        {
            return View("Index", new PruneResultViewModel
            {
                Networks = _dockerService.Prune(Models.PruneOption.Network)
            });
        }

        [Route("system")]
        public IActionResult PruneSystem()
        {
            return View("Index", new PruneResultViewModel
            {
                System = _dockerService.Prune(Models.PruneOption.System)
            });
        }
    }
}
