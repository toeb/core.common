using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Modules
{
  public class ApplicationContainerAttribute : ImportAttribute
  {
    public const string ContractName = "ApplicationContainer";
    public ApplicationContainerAttribute()
      : base(ContractName, typeof(CompositionContainer))
    {

    }
  }
}
