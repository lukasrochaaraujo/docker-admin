using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class AvailableImagesViewModel
    {
        public IEnumerable<ImageResumeModel> Images { get; set; }
    }
}
