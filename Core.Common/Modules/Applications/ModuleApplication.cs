using Core.FileSystem;
using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;

namespace Core.Modules.Applications
{
  [Export]
  public class ModuleApplication : ApplicationBase
  {
    protected ModuleApplication() : base(typeof(ModuleApplication)) { }
  }
}
