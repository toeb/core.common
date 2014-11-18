namespace Core.Modules.Installation
{

  public interface IInstallationService
  {
    bool HasInstallation(object installable);
    InstallationInstance RequireInstallation(object installable);
    InstallationInstance GetInstallation(object installable);   
    void Install(InstallationInstance installation);
    void Uninstall(InstallationInstance installation);
    bool CheckInstall(InstallationInstance installation);

    bool AddInstaller(IInstaller installer);
    bool RemoveInstaller(IInstaller installer);

  }
}