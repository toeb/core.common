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
using System.ComponentModel.Composition.Primitives;

namespace Core.Modules.Applications
{
  public static class IApplicationExtensions
  {

    /// <summary>
    /// returns activation order
    /// </summary>
    /// <param name="application"></param>
    /// <param name="modules"></param>
    /// <returns></returns>
    public static IEnumerable<IModuleInstance> ActivateModules(this IApplication application, IEnumerable<IModuleInstance> modulesToActivate)
    {

      List<IModuleInstance> activationOrder = new List<IModuleInstance>();
      Func<IModuleInstance, bool> activate = instance => { if (instance.IsActive)return true; try { application.ActivateModule(instance); activationOrder.Add(instance); } catch (Exception e) { } return instance.IsActive; };

      var modules = modulesToActivate.Where(module => module.ModuleInfo.AutoActivate && !module.IsActive).OrderBy(module => module.ModuleInfo.InitializationPriority).ToArray();

      int moduleCount = modules.Count();
      int lower = 0;
      int i = 0;

      // iterates through all modules 
      for (lower = 0; lower < moduleCount; lower++)
      {
        var module = modules[lower];
        // if module could be activateded just continue
        if (activate(module)) continue;

        bool found = false;
        // if module could not be activate try to activate the next possible inactive module
        for (i = lower + 1; i < moduleCount; i++)
        {
          if (!modules[i].IsActive && activate(modules[i]))
          {
            found = true;
            break;
          }
        }

        // if a succeeding module could be activated retry current module in next iteration (lower-- ++)
        if (found)
        {
          lower--;
          continue;
        }
        // if no succeeding module could be activated this module cannot be activated
        {
          var missingModules = modules.Except(activationOrder).ToArray(); ;

          string names = "";
          Exception rootException = null;
          var exceptions = missingModules.Select(missingModule =>
          {
            try
            {
              names += "\n" + missingModule.ModuleInfo.ModuleName;
              application.ActivateModule(missingModule);
              return null;
            }
            catch (Exception e)
            {
              if (missingModule == module)
              {
                rootException = e;
              }
              return e;
            }

          }).Where(e => e != null).ToArray();


          throw new AggregateException("could not activate modules: " + names + "\nbecause of " + module.ModuleInfo.ModuleName + " with error: " + rootException.Message, exceptions);
        }

      }
      return activationOrder;
    }

    /// <summary>
    /// activates all discovered modules by trying to activating each module until it works
    /// (so that dependencies are met)
    ///
    /// (can loop indefinetly needs gracefull error handling)
    /// </summary>
    /// <param name="application"></param>
    public static void ActivateAllDiscovered(this IApplication application)
    {
      application.ActivateModules(application.Modules);
    }

    public static void Compose(this IApplication application, object part)
    {
      var moduleApplication = application as ModuleApplication;
      if(moduleApplication==null)throw new InvalidOperationException("application must be a module application");
      moduleApplication.Container.ComposeParts(part);
    }
    /// <summary>
    /// imports a value into the application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="application"></param>
    /// <param name="value"></param>
    /// <param name="contractName"></param>
    /// <returns></returns>
    public static ComposablePart Import<T>(this ApplicationBase application, T value, string contractName = null) where T : class
    {
      var batch = new CompositionBatch();
      ComposablePart result;
      if (string.IsNullOrEmpty(contractName)) result = batch.AddExportedValue(value);
      else result = batch.AddExportedValue(contractName, value);
      application.Container.Compose(batch);
      return result;
    }
    /// <summary>
    /// uses the application to satisfy service imports
    /// </summary>
    /// <param name="self"></param>
    /// <param name="object"></param>
    public static void SatisfyImports(this IApplication self, object @object)
    {
      var application = self as ApplicationBase;
      if (application == null) throw new NotImplementedException("satisfying imports is only implemented for applications based on ApplicationBase");
      application.Container.SatisfyImportsOnce(@object);

    }
    public static T GetService<T>(this IApplication self)
    {
      var application = self as ApplicationBase;
      if (application == null) return default(T);
      var exports = application.Container.GetExports<T>();
      if (exports.Count() != 1) return default(T);
      return exports.Single().Value;
    }

    public static void PrintModules(this IApplication self)
    {
      foreach (var module in self.Modules)
      {
        Console.WriteLine(module.ModuleInfo);
      }
    }
    /// <summary>
    /// goes through all types and adds Module types to the application
    /// </summary>
    /// <param name="self"></param>
    public static void DiscoverModules(this IApplication self,string directory=null)
    {
      if (!string.IsNullOrEmpty(directory))
      {
        using (var container = new CompositionContainer(new DirectoryCatalog("."))) { }
      }
      
      var moduleTypes =
        ReflectionService.Instance
        .AllTypes
        .Where(t =>
          (t.HasAttribute<ModuleAttribute>(false))
          && !t.IsAbstract
          && !t.IsInterface);

      foreach (var type in moduleTypes)
      {
        var attribute = type.GetCustomAttribute<ModuleAttribute>();
        if (attribute != null && attribute.AutoDiscover == false) continue;
        self.AddModule(type);
      }
    }
    public static IModuleInstance FindModuleByPartialName(this IApplication self, string query)
    {
      return self.FindModulesByPartialName(query).FirstOrDefault();

    }

    public static IEnumerable<IModuleInstance> FindModulesByPartialName(this IApplication self, string query)
    {
      return self.Modules.Where(mi => mi.ModuleInfo.ModuleName.ToLower().Contains(query.ToLower()));
    }


    public static IEnumerable<IModuleInstance> FindModulesByNamespace(this IApplication self, string namespaceName)
    {
      return self.Modules.Where(mi => mi.ModuleInfo.Type.Namespace == namespaceName);
    }

    public static IEnumerable<IModuleInstance> FindAndActivateModules(this IApplication self, Func<IModuleInformation, bool> predicate)
    {
      var modules = self.Modules.Where(mi => predicate(mi.ModuleInfo));
      var activationOrder = self.ActivateModules(modules);
      var result = modules.Except(activationOrder).Concat(activationOrder);
      return result;
    }
    public static IEnumerable<IModuleInstance> FindAndActivateModulesByNamespace(this IApplication self, string namespaceName)
    {
      var modules = self.FindModulesByNamespace(namespaceName);
      var activationOrder = self.ActivateModules(modules);
      var result = modules.Except(activationOrder).Concat(activationOrder);
      return result;
    }



    /// <summary>
    /// finds modules by partial name
    /// </summary>
    /// <param name="self"></param>
    /// <param name="partialModuleName"></param>
    /// <returns>the order in which the modules were are activated</returns>
    public static IEnumerable<IModuleInstance> FindAndActivateModulesPartialName(this IApplication self, string partialModuleName)
    {
      var modules = self.FindModulesByPartialName(partialModuleName);
      var activationOrder = self.ActivateModules(modules);
      var result = modules.Except(activationOrder).Concat(activationOrder);
      return result;
    }

    /// <summary>
    /// registers the moduleType and tries to activate it
    /// </summary>
    /// <param name="self"></param>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public static IModuleInstance ActivateModuleType(this IApplication self, Type moduleType)
    {
      if (!self.HasModule(moduleType))
      {
        self.AddModule(moduleType);
        if (!self.HasModule(moduleType)) return null;
      }
      var module = self.GetModule(moduleType);
      self.ActivateModule(module);
      return module;
    }
    public static TModule ActivateModuleType<TModule>(this IApplication self)
    {
      var type = typeof(TModule);
      return (TModule)(self.ActivateModuleType(type).Module);
    }

    public static void AddModuleType<TModule>(this IApplication self)
    {
      var type = typeof(TModule);
      self.AddModule(type);
    }
  }
}
