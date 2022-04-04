using DockerAdmin.Interfaces;
using DockerAdmin.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DockerAdmin.Services
{
    public class DockerService : IDockerService
    {
        private readonly string CommandContainerListAllRunning = "docker container ls -f \\\"status=running\\\"";
        private readonly string CommandContainerListAllStopped = "docker container ls -f \\\"status=exited\\\"";
        private readonly string CommandContainerLogs = "docker container logs {0}";
        private readonly string CommandContainerRestart = "docker container restart {0}";
        private readonly string CommandContainerStart = "docker container start {0}";
        private readonly string CommandContainerStop = "docker container stop {0}";

        private readonly ILogger<DockerService> _logger;

        public DockerService(ILogger<DockerService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ContainerModel> GetAllRunningContainers()
        {
            var runningContainers = new List<ContainerModel>();

            try
            {
                string result = ExecuteDockerCommand(CommandContainerListAllRunning);

                var containers = result.Split("\n").ToList().Skip(1);

                foreach (var container in containers)
                {
                    var parts = Regex.Split(container, @"[\s+]{2,}");

                    if (parts.Length == 7)
                        runningContainers.Add(new ContainerModel
                        {
                            Id = parts[0],
                            Image = parts[1],
                            Command = parts[2],
                            Created = parts[3],
                            Status = parts[4],
                            Ports = parts[5],
                            Names = parts[6],
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get running containers: {ex.Message}");
            }

            return runningContainers;
        }

        public IEnumerable<ContainerModel> GetAllStoppedContainers()
        {
            var runningContainers = new List<ContainerModel>();

            try
            {
                string result = ExecuteDockerCommand(CommandContainerListAllStopped);

                var containers = result.Split("\n").ToList().Skip(1);

                foreach (var container in containers)
                {
                    var parts = Regex.Split(container, @"[\s+]{2,}");

                    if (parts.Length == 6 && parts[1].Contains(":"))
                        runningContainers.Add(new ContainerModel
                        {
                            Id = parts[0],
                            Image = parts[1],
                            Command = parts[2],
                            Created = parts[3],
                            Status = parts[4],
                            Names = parts[5],
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get stopped containers: {ex.Message}");
            }

            return runningContainers;
        }

        public string GetContainerLogs(string id)
        {
            try
            {
                string logs = ExecuteDockerCommand(string.Format(CommandContainerLogs, id));
                return logs?.TrimStart();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get container {id} logs: {ex.Message}");
            }

            return null;
        }

        public void RestartContainer(string id)
        {
            try
            {
                ExecuteDockerCommand(string.Format(CommandContainerRestart, id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to restart container {id}: {ex.Message}");
            }
        }

        public void StartContainer(string id)
        {
            try
            {
                ExecuteDockerCommand(string.Format(CommandContainerStart, id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to restart container {id}: {ex.Message}");
            }
        }

        public void StopContainer(string id)
        {
            try
            {
                ExecuteDockerCommand(string.Format(CommandContainerStop, id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to stop container {id}: {ex.Message}");
            }
        }

        private string ExecuteDockerCommand(string command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string commandOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return commandOutput;
        }
    }
}
