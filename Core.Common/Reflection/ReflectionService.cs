using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
  /// <summary>
  /// Default Reflection Provider
  /// </summary>
  [Export(typeof(IReflectionService))]
  public class ReflectionService : IReflectionService
  {
    private static ReflectionService instance;
    public static IReflectionService Instance
    {
      get
      {
        return instance ?? (instance = new ReflectionService());
      }
    }

    [ImportingConstructor]
    private ReflectionService()
    {
      instance = this;
      Refresh();
      AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoaded;
    }
    bool needsLoad = false;
    private void AssemblyLoaded(object sender, AssemblyLoadEventArgs args)
    {
      if (AssemblyAdded != null) AssemblyAdded(args.LoadedAssembly);
      if (TypesAdded != null) TypesAdded(args.LoadedAssembly.DefinedTypes);
      needsLoad = true;
    }
    void RefreshIfNeeded()
    {
      if (needsLoad)
      {
        needsLoad = false;
        Refresh();
      }
    }
    IQueryable<Assembly> assemblies;
    public IQueryable<System.Reflection.Assembly> AllAssemblies
    {
      get
      {
        RefreshIfNeeded();
        return assemblies;
      }
      private set
      {
        assemblies = value;
      }
    }

    IQueryable<Type> types;
    public IQueryable<Type> AllTypes
    {
      get
      {
        RefreshIfNeeded();
        return types;
      }
      private set
      {
        types = value;
      }
    }


    public void Refresh()
    {
      AllAssemblies = AppDomain.CurrentDomain.GetAssemblies().AsQueryable();
      AllTypes = AllAssemblies.SelectMany(assembly => assembly.GetTypes()).ToArray().AsQueryable();
      typesById.Clear();
      foreach (var type in AllTypes)
      {
        typesById[type.GUID] = type;
      }
    }

    public event Action<IEnumerable<Type>> TypesAdded;
    public event Action<Assembly> AssemblyAdded;

    private IDictionary<Guid, Type> typesById = new Dictionary<Guid, Type>();
    public Type GetById(Guid id)
    {
      RefreshIfNeeded();
      Type value;
      if (!typesById.TryGetValue(id, out value)) return null;
      return value;

    }
  }
}
