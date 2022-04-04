using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class AvailableContainersViewModel
    {
        public IEnumerable<ContainerResumeModel> ActiveContainers { get; set; }
        public IEnumerable<ContainerResumeModel> StoppedContainers { get; set; }
    }
}
