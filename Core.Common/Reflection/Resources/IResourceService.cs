
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Linq;
namespace Core.Common.Reflect.Resources
{
  public interface IResourceService
  {

    AssemblyResourceManager[] GetAllResourceManagers();
    AssemblyResourceManager GetResourceManager(Assembly assembly);
    object GetResource(object context, object identifier);
  }

}
