using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public static class IRulesServiceExtensions
  {
    public static object Get(this IRuleContext self, string key)
    {
      if (!self.Data.ContainsKey(key)) return null;
      return self.Data[key];
    }
    public static T Get<T>(this IRuleContext self, string key)
    {
      return (T)self.Get(key);
    }

    public static Task ApplyRules(this IRulesService self, object contextData)
    {
      return self.ApplyRules(new RuleContext(Anonymous.ToDictionary(contextData)));
    }

  }
}
