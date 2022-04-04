using DockerAdmin.Models;

namespace DockerAdmin.Interfaces
{
    public interface IDockerService
    {
        public IEnumerable<ContainerResumeModel> GetAllRunningContainers();
        public IEnumerable<ContainerResumeModel> GetAllStoppedContainers();
        public string GetContainerLogs(string id);
        public ContainerMetricsModel GetContainerMetrics(string id);
        public ContainerResumeModel GetContainerResumeInfo(string id);
        public string GetContainerSpecs(string id);
        public void StartContainer(string id);
        public void StopContainer(string id);
        public void RestartContainer(string id);
        public IEnumerable<ImageResumeModel> GetAllImages();
        void DeleteImage(string id);
        void UpdateImage(string id);
        public string Prune(PruneOption option);
    }
}
