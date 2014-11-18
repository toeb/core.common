using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Context
{
  [Module]
  public class ContextServiceModule
  {
    [ApplicationContainer]
    CompositionContainer ApplicationContainer { get; set; }

    ContextService ContextService { get; set; }

    [ActivationCallback]
    void Activate()
    {
      ContextService = new ContextService();
      ApplicationContainer.ComposeExportedValue(ContextService);
      ApplicationContainer.ComposeExportedValue<IContextService>(ContextService);
    }

  }
}
