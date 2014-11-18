using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  public static class ManagedObject
  {
    public static IExtensibleManagedObject Extensible(this object value)
    {
      AssignableManagedObject result = new AssignableManagedObject();
      result.PushPublicProperties(value, SinkToSourceMergeStrategy.Default);
      return result;
    }
    public static IExtensibleManagedObject Extensible()
    {
      AssignableManagedObject result = new AssignableManagedObject();

      return result;
    } 
  }
}
