using DockerAdmin.Interfaces;
using DockerAdmin.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DockerAdmin.Services
{
    public class DockerService : IDockerService
    {
        private readonly ILogger<DockerService> _logger;

        public DockerService(ILogger<DockerService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ContainerModel> GetAllRunningContainers()
        {
            var runningContainers = new List<ContainerModel>();

            var dockerContainerPsCommand = "docker container ps".Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{dockerContainerPsCommand}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

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

            return runningContainers;
        }

        public string GetContainerLogs(string id)
        {
            var dockerLogsCommand = $"docker logs {id}".Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{dockerLogsCommand}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            try
            {
                process.Start();
                var logs = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return logs?.TrimStart();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao requisitar logs do o container: {ex.Message}");
            }

            return null;
        }

        public void RestartContainer(string id)
        {
            var dockerRestartCommand = $"docker restart {id}".Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{dockerRestartCommand}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            try
            {
                process.Start();
                _ = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao reiniciar o container: {ex.Message}");
            }
        }
    }
}
