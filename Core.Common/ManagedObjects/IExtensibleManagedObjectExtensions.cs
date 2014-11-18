using Core.Extensions;

namespace Core.ManagedObjects
{
  public static class IExtensibleManagedObjectExtensions
  {
    /// <summary>
    /// creates a object property which is readable and writable
    /// </summary>
    /// <param name="self"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IManagedProperty RequireProperty(this IExtensibleManagedObject self, string name)
    {
      return self.RequireProperty(new ManagedPropertyInfo(name,true,true,typeof(object),false), ((object)null).MakeLazy());
    }
  }
}
