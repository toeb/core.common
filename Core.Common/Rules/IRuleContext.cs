using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public interface IRuleContext
  {
    string Action { get; }
    string ExtensionPoint { get; }
    object Resource { get; }
    IDictionary<string, object> Data { get; }
  }
}
