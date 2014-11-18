using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public interface IRule
  {
    bool Applies(IRuleContext context);
    Task Apply(IRuleContext context);
  }

}
