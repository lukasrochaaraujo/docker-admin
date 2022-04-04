namespace DockerAdmin.Models
{
    public class ContainerMetricsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CpuUsage { get; set; }
        public string MemoryUsage { get; set; }
        public string MemoryLimit { get; set; }
        public string MemoryPercent { get; set; }
        public string NetworkInput { get; set; }
        public string NetworkOutput { get; set; }
        public string DiskInput { get; set; }
        public string DiskOutput { get; set; }
        public string Pids { get; set; }
    }
}
