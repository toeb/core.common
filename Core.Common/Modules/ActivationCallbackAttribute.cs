using System;
using System.ComponentModel.Composition;

namespace Core.Modules
{
  [AttributeUsage(AttributeTargets.Method)]
  public class ActivationCallbackAttribute : ExportAttribute
  {
    public const string ContractName = "ModuleActivationCallback";
    public ActivationCallbackAttribute()
      : base(ContractName)
    {

    }
  }
}
