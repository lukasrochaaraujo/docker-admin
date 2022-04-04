using DockerAdmin.Interfaces;
using DockerAdmin.Models;
using DockerAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DockerAdmin.Controllers
{
    public class ContainersController : Controller
    {
        private readonly IDockerService _dockerService;

        public ContainersController(IDockerService dockerService)
        {
            _dockerService = dockerService;
        }

        public IActionResult Index()
        {
            return View(new ActiveContainersViewModel
            {
                ActiveContainers = _dockerService.GetAllRunningContainers(),
                StoppedContainers = _dockerService.GetAllStoppedContainers()
            });
        }

        [Route("start/{id}")]
        public IActionResult StartContainer(string id)
        {
            _dockerService.StartContainer(id);
            return RedirectToAction("Index");
        }

        [Route("stop/{id}")]
        public IActionResult StopContainer(string id)
        {
            _dockerService.StopContainer(id);
            return RedirectToAction("Index");
        }

        [Route("restart/{id}")]
        public IActionResult RestartContainer(string id)
        {
            _dockerService.RestartContainer(id);
            return RedirectToAction("Index");
        }

        [Route("details/{id}")]
        public IActionResult DetailsContainer(string id)
        {
            return View("Details", new ContainerDetailsViewModel
            {
                Resume = _dockerService.GetContainerResumeInfo(id),
                Metrics = _dockerService.GetContainerMetrics(id),
                Specs = _dockerService.GetContainerSpecs(id)
            });
        }

        [Route("logs/{id}")]
        public IActionResult LogsContainer(string id)
        {
            return View("Logs", new ContainerLogsViewModel
            {
                ContainerId = id,
                Logs = _dockerService.GetContainerLogs(id)
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}