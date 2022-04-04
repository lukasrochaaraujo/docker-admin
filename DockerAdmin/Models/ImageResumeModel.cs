namespace DockerAdmin.Models
{
    public class ImageResumeModel
    {
        public string Repository { get; set; }
        public string Tag { get; set; }
        public string Id { get; set; }
        public string Created { get; set; }
        public string Size { get; set; }
    }
}
