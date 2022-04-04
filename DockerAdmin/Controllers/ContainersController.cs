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
            var viewModel = new ActiveContainersViewModel();
            viewModel.ActiveContainers = _dockerService.GetAllRunningContainers();
            return View(viewModel);
        }

        [Route("restart/{id}")]
        public IActionResult RestartContainer(string id)
        {
            _dockerService.RestartContainer(id);
            return RedirectToAction("Index");
        }

        [Route("metrics/{id}")]
        public IActionResult MetricsContainer(string id)
        {
            _dockerService.GetContainerLogs(id);
            return RedirectToAction("Index");
        }

        [Route("logs/{id}")]
        public IActionResult LogsContainer(string id)
        {
            var logs = _dockerService.GetContainerLogs(id);
            return View("Logs", new ContainerLogsViewModel
            {
                ContainerId = id,
                Logs = logs
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}