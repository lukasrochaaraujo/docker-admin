using DockerAdmin.Models;

namespace DockerAdmin.Interfaces
{
    public interface IDockerService
    {
        public IEnumerable<ContainerModel> GetAllRunningContainers();
        public IEnumerable<ContainerModel> GetAllStoppedContainers();
        public string GetContainerLogs(string id);
        public void StartContainer(string id);
        public void StopContainer(string id);
        public void RestartContainer(string id);
    }
}
