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
        private readonly string CommandContainerInfo = "docker container ls -a | grep {0}";
        private readonly string CommandContainerMetrics = "docker container stats {0} --no-stream";
        private readonly string CommandContainerSpecs = "docker container inspect {0}";
        private readonly string CommandContainerLogs = "docker container logs {0}";
        private readonly string CommandContainerRestart = "docker container restart {0}";
        private readonly string CommandContainerStart = "docker container start {0}";
        private readonly string CommandContainerStop = "docker container stop {0}";
        private readonly string CommandPrune = "docker {0} prune -f";

        private readonly ILogger<DockerService> _logger;

        public DockerService(ILogger<DockerService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ContainerResumeModel> GetAllRunningContainers()
        {
            var runningContainers = new List<ContainerResumeModel>();

            try
            {
                string result = ExecuteDockerCommand(CommandContainerListAllRunning);

                var containers = result.Split("\n").ToList().Skip(1);

                foreach (var container in containers)
                {
                    var parts = Regex.Split(container, @"[\s+]{2,}");

                    if (parts.Length == 7)
                        runningContainers.Add(new ContainerResumeModel
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

        public IEnumerable<ContainerResumeModel> GetAllStoppedContainers()
        {
            var runningContainers = new List<ContainerResumeModel>();

            try
            {
                string result = ExecuteDockerCommand(CommandContainerListAllStopped);

                var containers = result.Split("\n").ToList().Skip(1);

                foreach (var container in containers)
                {
                    var parts = Regex.Split(container, @"[\s+]{2,}");

                    if (parts.Length == 6 && parts[1].Contains(":"))
                        runningContainers.Add(new ContainerResumeModel
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
                return ExecuteDockerCommand(string.Format(CommandContainerLogs, id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get container {id} logs: {ex.Message}");
            }

            return null;
        }

        public ContainerMetricsModel GetContainerMetrics(string id)
        {
            try
            {
                var containerStatsString = ExecuteDockerCommand(string.Format(CommandContainerMetrics, id));

                var containerStatsList = containerStatsString.Split("\n").ToList().Skip(1);

                foreach (var containerStats in containerStatsList)
                {
                    var parts = Regex.Split(containerStats, @"[\s+]{2,}");

                    if (parts.Length == 8)
                    {
                        var memoryParts = parts[3].Split("/");
                        var networkParts = parts[5].Split("/");
                        var diskParts = parts[6].Split("/");

                        return new ContainerMetricsModel
                        {
                            Id = parts[0],
                            Name = parts[1],
                            CpuUsage = parts[2],
                            MemoryUsage = memoryParts[0],
                            MemoryLimit = memoryParts[1],
                            MemoryPercent = parts[4],
                            NetworkInput = networkParts[0],
                            NetworkOutput = networkParts[1],
                            DiskInput = diskParts[0],
                            DiskOutput = diskParts[1]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get container {id} stats: {ex.Message}");
            }

            return default;
        }

        public ContainerResumeModel GetContainerResumeInfo(string id)
        {
            try
            {
                var resumeAsString = ExecuteDockerCommand(string.Format(CommandContainerInfo, id));

                var parts = Regex.Split(resumeAsString, @"[\s+]{2,}");

                if (parts.Length == 7)
                    return new ContainerResumeModel
                    {
                        Id = parts[0],
                        Image = parts[1],
                        Command = parts[2],
                        Created = parts[3],
                        Status = parts[4],
                        Ports = parts[5],
                        Names = parts[6],
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get container {id} resumed info: {ex.Message}");
            }

            return default;
        }

        public string GetContainerSpecs(string id)
        {
            try
            {
                return ExecuteDockerCommand(string.Format(CommandContainerSpecs, id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get container {id} specs: {ex.Message}");
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

        public string Prune(PruneOption option)
        {
            try
            {
                string commandResult = ExecuteDockerCommand(string.Format(CommandPrune, option.ToString().ToLower()));

                if (string.IsNullOrWhiteSpace(commandResult))
                    return "Success!";

                if (commandResult.Split("\n").Length <= 1)
                    return $"Sucess: {commandResult}!";

                return "Success: " + commandResult
                    .Split("\n")
                    .ToHashSet()
                    .Reverse()
                    .Skip(1)
                    .First()
                    + "!";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to prune images: {ex.Message}");
            }

            return "Fail!";
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
