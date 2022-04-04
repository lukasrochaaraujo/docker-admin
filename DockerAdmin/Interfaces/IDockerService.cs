using DockerAdmin.Models;

namespace DockerAdmin.Interfaces
{
    public interface IDockerService
    {
        public IEnumerable<ContainerModel> GetAllRunningContainers();
        public void RestartContainer(string id);
        public string GetContainerLogs(string id);
    }
}
