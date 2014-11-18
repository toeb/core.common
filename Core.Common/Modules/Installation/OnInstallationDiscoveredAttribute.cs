using System;
using System.ComponentModel.Composition;

namespace Core.Modules.Installation
{
  [AttributeUsage(AttributeTargets.Method)]
  public class OnInstallationDiscoveredAttribute:ExportAttribute
  {
    public const string ContractName = "OnInstallationDiscovered";
    public OnInstallationDiscoveredAttribute():base(ContractName) { }
  }
}