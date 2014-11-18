using Core.FileSystem;
using System.Collections.Generic;

namespace Core.Modules.Installation
{
    public class InstallationInstance
    {
        public InstallationInstance() { InstallationAspects = new Dictionary<string, IInstallationAspect>(); }
        public IRelativeFileSystem InstallationFileSystem { get; set; }
        public string InstallationDirectory { get; set; }
        public bool IsInstalled { get; set; }
        public object Installable { get; set; }
        public InstallableAttribute InstallationInformation { get; set; }
        public IInstallationService InstallationService { get; set; }

        public IDictionary<string, IInstallationAspect> InstallationAspects { get; set; }



    }
}
