using System.Reflection;

namespace Core.Resources
{
  public interface IAssemblyResourceService
  {
    /// <summary>
    /// parses resources of specified assembly
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    IAssemblyResources GetResources(Assembly assembly);
  }
}
