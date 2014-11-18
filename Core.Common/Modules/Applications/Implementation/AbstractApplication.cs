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
using System.Threading;

namespace Core.Modules.Applications
{
  [InheritedExport]
  [InheritedExport(typeof(IApplication))]
  [InheritedExport("Application", typeof(IApplication))]
  public abstract class AbstractApplication : NotifyPropertyChangedBase, IApplication
  {
    private IDictionary<Type, IModifiableModuleInstance> modules = new Dictionary<Type,IModifiableModuleInstance>();
  
    public AbstractApplication(IApplicationInformation info)
    {
      ApplicationInfo = info;
    }
    public void RemoveModule(Type moduleType)
    {
      if (!HasModule(moduleType)) return;
      var instance = GetModule(moduleType);
      if (instance.IsActive)
      {
        DeactivateModule(instance);
      }
      modules.Remove(moduleType);
    }
    public void AddModule(Type moduleType)
    {
      if(HasModule(moduleType))return;
      NotifyModuleDiscovered(moduleType);
    }
    public bool HasModule(Type moduleType)
    {
      return modules.ContainsKey(moduleType);
    }
    public IModuleInstance GetModule(Type moduleType)
    {
      if (!HasModule(moduleType)) return null;
      return modules[moduleType];
    }


    /// <summary>
    /// a list of available module instances
    /// </summary>
    [Export("ApplicationModules")]
    public IEnumerable<IModuleInstance> Modules
    {
      get { return modules.Values; }
    }

    /// <summary>
    /// subclasses may override and return whether the moduel is to be accepted or not
    /// if the type is accepted the application will create a corresponding IModuleInstance
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    protected virtual bool AcceptModule(Type moduleType) { return true; }

    /// <summary>
    /// subclasses must implement. the must return the moduleinformation for the specified type
    /// (this allows non instanciated types to be reflected
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected abstract IModuleInformation GetModuleInformation(Type type);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    protected abstract void ActivateModuleInstance(IModuleInstance instance);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    protected abstract void DeactivateModuleInstance(IModuleInstance instance);

    /// <summary>
    /// subclasses may override to construct a custom module instance
    /// </summary>
    /// <param name="information"></param>
    /// <returns></returns>
    protected virtual IModifiableModuleInstance ConstructModuleInstance(IModuleInformation information)
    {
      return new ModuleInstance(this, information);
    }

    /// <summary>
    /// returns the module instance for the specified type creating it if it does not exist
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    private IModuleInstance RegisterModule(Type moduleType)
    {
      if (!AcceptModule(moduleType)) return null;
      var moduleInfo = GetModuleInformation(moduleType);
      var result = ConstructModuleInstance(moduleInfo);
      modules[moduleType] = result;
      OnModuleRegistered(result);
      return result;
    }


    /// <summary>
    /// subclasses call this to notify the application that it discovered a new module type
    /// </summary>
    /// <param name="moduleType"></param>
    protected void NotifyModuleDiscovered(Type moduleType)
    {
      RegisterModule(moduleType);
    }

    /// <summary>
    /// activates the module specified
    /// can throw if activation fails
    /// </summary>
    /// <param name="instance"></param>
    [Export("ActivateModule")]
    public void ActivateModule(IModuleInstance module)
    {
      if (module.IsActive) return;
      var instance = module as IModifiableModuleInstance;
      try
      {
        ActivateModuleInstance(instance);
      }
      catch(Exception e)
      {
        instance.Module = null;
        throw new Exception("could not activate module "+module.ModuleInfo.ModuleName+" see inner exception.  Message:"+e.Message,e);
      }
      instance.IsActive = true;
      OnModuleActivated(instance);
    }

    /// <summary>
    /// deactivates the module specified
    /// can throw if the module cannot be deactivated
    /// </summary>
    /// <param name="instance"></param>
    [Export("DeactivateModule")]
    public void DeactivateModule(IModuleInstance module)
    {
      if (!module.IsActive) return;
      var instance = module as IModifiableModuleInstance;
      instance.IsActive = false;
      try
      {
        DeactivateModuleInstance(instance);
      }
      catch
      {
        instance.IsActive = true;
        throw;
      }
      OnModuleDeactivated(instance);
    }

    [Export("ApplicationInfo")]
    [Export]
    public IApplicationInformation ApplicationInfo
    {
      get;
      private set;
    }


    #region extension points

    protected virtual void OnModuleRegistered(IModuleInstance result) { }
    protected virtual void OnModuleActivated(IModuleInstance instance) { }
    protected virtual void OnModuleDeactivated(IModuleInstance instance) { }
    protected virtual void OnApplicationStarted() { }
    protected virtual void OnApplicationStopped() { }

    #endregion

  }
}
