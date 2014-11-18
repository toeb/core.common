using System.ComponentModel.Composition;

namespace Core.Modules.Installation
{
  public class CanInstallCallbackAttribute : ExportAttribute
  {
    public const string ContractName = "CanInstallCallback";
    public CanInstallCallbackAttribute() : base(ContractName) { }

  }
}
