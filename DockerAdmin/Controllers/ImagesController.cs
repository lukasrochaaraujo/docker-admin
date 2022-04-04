using DockerAdmin.Interfaces;
using DockerAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DockerAdmin.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IDockerService _dockerService;

        public ImagesController(IDockerService dockerService)
        {
            _dockerService = dockerService;
        }

        public IActionResult Index()
        {
            return View(new AvailableImagesViewModel
            {
                Images = _dockerService.GetAllImages()
            });
        }

        [Route("delete/{id}")]
        public IActionResult DeleteImage(string id)
        {
            _dockerService.DeleteImage(id);
            return RedirectToAction("Index");
        }

        [Route("update/{id}")]
        public IActionResult UpdateImage(string id)
        {
            _dockerService.UpdateImage(id);
            return RedirectToAction("Index");
        }
    }
}
