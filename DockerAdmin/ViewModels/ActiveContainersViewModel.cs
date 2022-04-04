using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class ActiveContainersViewModel
    {
        public IEnumerable<ContainerResumeModel> ActiveContainers { get; set; }
        public IEnumerable<ContainerResumeModel> StoppedContainers { get; set; }
    }
}
