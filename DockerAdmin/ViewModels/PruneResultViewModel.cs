namespace DockerAdmin.ViewModels
{
    public class PruneResultViewModel
    {
        public string Images { get; set; }
        public string Containers { get; set; }
        public string Volumes { get; set; }
        public string Networks { get; set; }
        public string System { get; set; }
    }
}
