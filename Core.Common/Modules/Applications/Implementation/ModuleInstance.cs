using Core.Collections;
using Core.Graph;
using Core.Graph.Directed;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
using Core.FileSystem;
using Core.Resources;
using System.Diagnostics;

namespace Core.Modules.Applications
{
  [DebuggerDisplay("{ModuleInfo}  Active: {IsActive}")]
  public class ModuleInstance : NotifyPropertyChangedBase, IModuleInstance, IModifiableModuleInstance
  {
    public ModuleInstance(IApplication application, IModuleInformation information)
    {
     
      if (information == null) throw new ArgumentNullException();
      Application = application;
      ModuleInfo = information;
    }
    public IModuleInformation ModuleInfo { get; set; }
    public CompositionContainer ModuleContainer { get; set; }
    public object Module { get; set; }




    public Type Id
    {
      get { return ModuleInfo.Type; }
    }
    

    Guid IIdentifiable<Guid>.Id
    {
      get { return ModuleInfo.Id; }
    }

    private bool isActive = false;
    public bool IsActive
    {
      get { return isActive; }
      set { ChangeIfDifferent(ref isActive, value, "IsActive"); }
    }

    public IApplication Application
    {
      get;private set; 
    }



    public System.ComponentModel.Composition.Primitives.ComposablePart Part { get; set; }


  }
}
