using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public interface IRulesService
  {
    void AddRule(IRule rule);
    void RemoveRule(IRule rule);
    Task ApplyRules(IRuleContext context);
  }
}
