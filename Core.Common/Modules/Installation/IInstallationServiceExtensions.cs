using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Installation
{
  public static class IInstallationServiceExtensions
  {
    /// <summary>
    /// installs the specified object
    /// </summary>
    /// <param name="self"></param>
    /// <param name="installableObject"></param>
    public static void Install(this IInstallationService self, object installableObject)
    {
      var instance = self.GetInstallation(installableObject);
      if (instance == null) return;
      self.Install(instance);
    }
  }
}
