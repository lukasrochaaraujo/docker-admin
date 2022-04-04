using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class ContainerLogsViewModel
    {
        public string ContainerId { get; set; }
        public string Logs { get; set; }
    }
}
