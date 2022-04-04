namespace DockerAdmin.Models
{
    public class ContainerModel
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Command { get; set; }
        public string Created { get; set; }
        public string Status { get; set; }
        public string Ports { get; set; }
        public string Names { get; set; }
    }
}
