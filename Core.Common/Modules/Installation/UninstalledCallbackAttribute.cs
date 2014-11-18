using System;
using System.ComponentModel.Composition;

namespace Core.Modules.Installation
{
  [AttributeUsage(AttributeTargets.Method)]
  public class UninstalledCallbackAttribute : ExportAttribute
  {
    public const string ContractName = "OnUninstallCallback";

    /// <summary>
    /// only works on public methdos currently
    /// </summary>
    public UninstalledCallbackAttribute() : base(ContractName) { }

  }
}
