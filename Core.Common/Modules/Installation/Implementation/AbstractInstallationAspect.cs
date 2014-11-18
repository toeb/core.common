namespace Core.Modules.Installation
{
  public class AbstractInstallationAspect : IInstallationAspect
  {
    public InstallationInstance Installation { get; set; }
    public IInstaller Installer { get; set; }
    public IInstallationService InstallationService { get; set; }


    public string AspectName
    {
      get { return Installer.GetType().FullName; }
    }
  }
}