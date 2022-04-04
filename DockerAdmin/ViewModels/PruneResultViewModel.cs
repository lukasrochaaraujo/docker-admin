using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class PruneResultViewModel
    {
        public PruneResultViewModel() 
        { }

        public PruneResultViewModel(PruneOption option, string result)
        {
            switch (option)
            {
                case PruneOption.System:
                    System = result;
                    break;
                case PruneOption.Image:
                    Images = result;
                    break;
                case PruneOption.Container:
                    Containers = result;
                    break;
                case PruneOption.Volume:
                    Volumes = result;
                    break;
                case PruneOption.Network:
                    Networks = result;
                    break;
            }
        }

        public string Images { get; set; }
        public string Containers { get; set; }
        public string Volumes { get; set; }
        public string Networks { get; set; }
        public string System { get; set; }
    }
}
