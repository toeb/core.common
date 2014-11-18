using System;
using System.ComponentModel.Composition;

namespace Core.Modules.Installation
{
  /// <summary>
  /// the void() method decorated with this attribute is called after the object is installed/ or an valid installation was found
  /// 
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class InstalledCallbackAttribute : ExportAttribute
  {
    public const string ContractName = "OnInstalledCallback";

    /// <summary>
    /// only works on public methdos currently
    /// </summary>
    public InstalledCallbackAttribute() : base(ContractName) { }
  }
}
