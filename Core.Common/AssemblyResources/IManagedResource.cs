using System.Reflection;

namespace Core.Resources
{
  public interface IManagedResource : IIdentifiable<string>
  {
    Assembly Assembly { get; }
  }
}
