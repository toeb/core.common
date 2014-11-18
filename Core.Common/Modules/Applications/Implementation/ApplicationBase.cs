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
using Newtonsoft.Json;
using System.IO;
using Core.Extensions;
using System.ComponentModel.Composition.Primitives;
namespace Core.Modules.Applications
{


  public class ApplicationBase : AbstractApplication
  {

    private void SetModuleContainer(ModuleInstance instance)
    {

      if (instance.Module == null) return;
      instance.Module.SetAttributedPropertyValues<ModuleContainerAttribute>(instance.ModuleContainer, false, true);
    }


    private void UnsetModuleContainer(ModuleInstance instance)
    {

      if (instance.Module == null) return;
      instance.Module.SetAttributedPropertyValues<ModuleContainerAttribute>(null, false, true);
    }

    private void UnsetModuleInstance(ModuleInstance instance)
    {

      if (instance.Module == null) return;
      instance.Module.SetAttributedPropertyValues<ModuleInstanceAttribute>(null, false, true);
    }
    private void SetModuleInstance(ModuleInstance instance)
    {
      if (instance.Module == null) return;
      instance.Module.SetAttributedPropertyValues<ModuleInstanceAttribute>(instance, false, true);
    }
    public ApplicationBase(IApplicationInformation info) : base(info) { }
    public ApplicationBase(Type type)
      : this(new ApplicationInformation(type, null, null, null))
    {
    }

    CompositionContainer container;
    [Import]
    public CompositionContainer Container
    {
      set
      {
        if (container == value) return;
        container = value;
        container.ComposeExportedValue<CompositionContainer>(ApplicationContainerAttribute.ContractName, container);
        SetupApplicationContainer(container);
      }
      get { return container; }
    }

    protected virtual void SetupApplicationContainer(CompositionContainer container)
    {

    }



    protected override IModuleInformation GetModuleInformation(Type type)
    {
      var infoAttribute = type.GetCustomAttribute<ModuleAttribute>();
      if (infoAttribute == null) throw new ArgumentException("type does not have a module attribute");

      var version = new Version(1, 0, 0, 0);
      var id = type.GUID;
      var name = type.FullName;

      if (!string.IsNullOrEmpty(infoAttribute.ModuleName)) name = infoAttribute.ModuleName;
      if (!string.IsNullOrEmpty(infoAttribute.ModuleGuid)) id = Guid.Parse(infoAttribute.ModuleGuid);
      if (!string.IsNullOrEmpty(infoAttribute.ModuleVersion)) version = new Version(infoAttribute.ModuleVersion);

      ModuleInformaton info = new ModuleInformaton(type, name, id, version);
      info.AutoActivate = infoAttribute.AutoActivate;
      info.InitializationPriority = infoAttribute.InitializationPriority;


      return info;
    }

    protected override IModifiableModuleInstance ConstructModuleInstance(IModuleInformation information)
    {
      return base.ConstructModuleInstance(information);
    }
    protected override void ActivateModuleInstance(IModuleInstance module)
    {
      var instance = module as ModuleInstance;
      var moduleType = module.ModuleInfo.Type;
      // step 1) create a container
      instance.ModuleContainer = new CompositionContainer(new TypeCatalog(moduleType), Container);

      // step 2) create module instance 

      // throws exception
      var export = instance.ModuleContainer.GetExport<object>(moduleType.FullName);
      instance.Module = export.Value;


      SetModuleInstance(instance);
      SetModuleContainer(instance);


      // step 3) callback module that it was activate
      NotifyModuleOfActivation(instance);

      // step 4) export activated module to application container so its services are exposed
      var batch = new CompositionBatch();
      var part = batch.AddPart(instance.Module);

      //throws exception
      Container.Compose(batch);

      //store part so it can be removed later
      instance.Part = part;


      // module activation complete
    }


    private void NotifyModuleOfActivation(ModuleInstance instance)
    {

      var callbacks = instance.Module.GetExportedDelegates(ActivationCallbackAttribute.ContractName);
      if (!callbacks.Any())
      {
        var imodule = instance.Module as IModule;
        if (imodule == null) return;
        imodule.Activate();
        return;
      }
      callbacks.Do(callback => callback.DynamicInvoke());
    }


    private void NotifyModuleOfDeactivation(ModuleInstance instance)
    {

      var callbacks = instance.Module.GetExportedDelegates(DeactivationCallbackAttribute.ContractName);
      if (!callbacks.Any())
      {
        var imodule = instance.Module as IModule;
        if (imodule == null) return;
        imodule.Deactivate();
        return;
      }
      callbacks.Do(callback => callback.DynamicInvoke());

    }




    protected override void DeactivateModuleInstance(IModuleInstance module)
    {
      var instance = module as ModuleInstance;

      // step 1) remove part from appliation container
      var batch = new CompositionBatch();
      batch.RemovePart(instance.Part);
      Container.Compose(batch);
      instance.Part = null;


      // step 2) callback module that it was deactivated
      NotifyModuleOfDeactivation(instance);


      UnsetModuleInstance(instance);
      UnsetModuleContainer(instance);

      //step 3) dispose module instance
      instance.Module = null;



      // step 4 ) dispose module container
      instance.ModuleContainer.Dispose();
      instance.ModuleContainer = null;

    }

  }
}
