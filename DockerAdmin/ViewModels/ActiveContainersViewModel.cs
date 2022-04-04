﻿using DockerAdmin.Models;

namespace DockerAdmin.ViewModels
{
    public class ActiveContainersViewModel
    {
        public IEnumerable<ContainerModel> ActiveContainers { get; set; }
        public IEnumerable<ContainerModel> StoppedContainers { get; set; }
    }
}