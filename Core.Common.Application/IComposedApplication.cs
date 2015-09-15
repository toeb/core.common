using System.ComponentModel.Composition.Hosting;

namespace Core.Common.Applications
{
  public interface IComposedApplication
  {
    CompositionContainer Container{ get; }
  }

 
}
