using System;
using System.ComponentModel.Composition;

namespace Core.Modules
{
  [AttributeUsage(AttributeTargets.Method)]
  public class DeactivationCallbackAttribute : ExportAttribute
  {
    public const string ContractName = "ModuleDeactivationCallback";
    public DeactivationCallbackAttribute()
      : base(ContractName)
    {

    }
  }
}
