using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Linq;

namespace Core.Common.Reflect.Resources
{
  public class AssemblyResourceManager
  {
    public Assembly Assembly { get; set; }
    public ResourceManager ResourceManager { get; set; }
  }
}
