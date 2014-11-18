using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Modules.Rules
{
  class DummyRulesService : IRulesService
  {
    public void AddRule(IRule rule)
    {
      throw new InvalidOperationException("dummy service does not allow rules");
    }

    public void RemoveRule(IRule rule)
    {

      throw new InvalidOperationException("dummy service does not allow rules");
    }

    public async System.Threading.Tasks.Task ApplyRules(IRuleContext context)
    {
      
    }
  }
}
