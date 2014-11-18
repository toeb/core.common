using System.Collections.Generic;

namespace Core.Modules.Installation
{
  public interface IInstaller
  {
    /// <summary>
    /// needs to return all installation requirements that this installer supports
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IEnumerable<string> CanInstall(InstallationInstance instance);
    /// <summary>
    /// needs to return true the installation instance is installed according to this installers logic
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    bool CheckInstall(InstallationInstance instance);
    /// <summary>
    /// needs to ensure that the instance is installed
    /// </summary>
    /// <param name="instance"></param>
    void Install(InstallationInstance instance);
    /// <summary>
    /// needs to ensure that the instance is not installed
    /// </summary>
    /// <param name="instance"></param>
    void Uninstall(InstallationInstance instance);
  }
}
