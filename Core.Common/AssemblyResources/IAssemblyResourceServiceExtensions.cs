using System;

namespace Core.Resources
{
  public static class IAssemblyResourceServiceExtensions
  {
    /// <summary>
    /// returns the assembly resources of the assembly where T is defined
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static IAssemblyResources GetResources<T>(this IAssemblyResourceService self)
    {
      return self.GetResources(typeof(T));
    }
    /// <summary>
    /// returns the assembly resources of the assembly where type is defined
    /// </summary>
    /// <param name="self"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IAssemblyResources GetResources(this IAssemblyResourceService self, Type type)
    {
      var assembly = type.Assembly;
      return self.GetResources(assembly);
    }
  }
}