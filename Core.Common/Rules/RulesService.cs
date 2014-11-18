using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public class RulesService : IRulesService
  {
    public static readonly IRulesService Dummy = new DummyRulesService();
    public RulesService() { Rules = new List<IRule>(); }
    private List<IRule> Rules { get; set; }
    public void AddRule(IRule rule)
    {
      Rules.Add(rule);
    }

    public void RemoveRule(IRule rule)
    {
      Rules.Remove(rule);
    }
    public async Task ApplyRules(IRuleContext context)
    {
      var ruleOrder = Rules.Where(r => r.Applies(context)).OrderByDescending(r => Priority.GetPriority(r)).ToArray();
      foreach (var rule in ruleOrder)
      {
        if (rule.Applies(context))
        {
          await rule.Apply(context);
        }
      }
    }
  }
}
