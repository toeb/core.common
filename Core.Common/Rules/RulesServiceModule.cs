
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
namespace Core.Modules.Rules
{



  [Module]
  public class RulesServiceModule
  {
    [Export(typeof(IRulesService))]
    [Export]
    RulesService Service { get; set; }
    [ActivationCallback]
    void Activate()
    {
      Service = new RulesService();
    }
  }
}
