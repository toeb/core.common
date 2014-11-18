using System.ComponentModel.Composition;

namespace Core.Modules.Installation
{
  public class InstallerAttribute : ExportAttribute
  {
    public InstallerAttribute()
      : base(typeof(IInstaller))
    {

    }
  }
}
