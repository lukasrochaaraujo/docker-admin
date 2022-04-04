using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class ContainerDetailsViewModel
    {
        public ContainerResumeModel Resume { get; set; }
        public ContainerMetricsModel Metrics { get; set; }
        public string Specs { get; set; }
    }
}
