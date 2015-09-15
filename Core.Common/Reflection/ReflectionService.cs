using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Core.Common.Reflect
{
  [Export(typeof(IReflectionService))]
  public class ReflectionService : IReflectionService
  {
    private static IDictionary<Assembly, IEnumerable<Type>> types = new Dictionary<Assembly, IEnumerable<Type>>();


    static ReflectionService()
    {
      AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoaded;
      foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
      {
        Announce(asm);
      }
    }
    public IEnumerable<Assembly> Assemblies
    {
      get
      {
        lock (types)
        {
          return types.Keys.ToArray();
        }
      }
    }
    public IEnumerable<Type> Types
    {
      get
      {
        lock (types)
        {
          return types.Values.SelectMany(t => t).ToArray();
        }
      }
    }
    private static void AssemblyLoaded(object sender, AssemblyLoadEventArgs args)
    {
      Announce(args.LoadedAssembly);
    }
  
    private static void Announce(Assembly assembly)
    {
      lock (types)
      {
        if (types.ContainsKey(assembly)) return;
        types[assembly] = assembly.GetTypes();
      }
    }
   
  
  }
}
