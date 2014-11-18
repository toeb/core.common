using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
  public interface IReflectionService
  {
    event Action<IEnumerable<Type>> TypesAdded;
    event Action<Assembly> AssemblyAdded;

    /// <summary>
    /// All assemblies in current app domain
    /// </summary>
    IQueryable<Assembly> AllAssemblies { get; }
    /// <summary>
    /// All types in curretn app domain
    /// </summary>
    IQueryable<Type> AllTypes { get; }
    /// <summary>
    /// returns the type uniquely identified by guid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Type GetById(Guid id);
    /// <summary>
    /// Refreshes the types and assemblies which are loaded in the current app domains
    /// </summary>
    void Refresh();
  }
}
