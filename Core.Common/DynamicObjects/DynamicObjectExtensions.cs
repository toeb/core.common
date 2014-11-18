using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.DynamicObjects
{

  public static class Extensions
  {
    /// <summary>
    /// fills the dictionary with the properites of object, object may also be a dynamic object
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="object"></param>
    public static void FillDictionary(IDictionary<string, object> dictionary, object @object)
    {
      if (@object == null) return;
      var metaObjectProvider = @object as IDynamicMetaObjectProvider;


      if (metaObjectProvider == null)
      {
        foreach (var property in @object.GetType().GetProperties().Where(prop => prop.CanRead))
        {
          dictionary[property.Name] = property.GetValue(@object);
        }
        return;
      }


      var metaObject = metaObjectProvider.GetMetaObject(Expression.Constant(@object));

      foreach (var member in metaObject.GetDynamicMemberNames())
      {
        dictionary[member] = GetPropertyValue(@object, member);
      }
    }
    /// <summary>
    /// very slow method to get a dictionary from a possibly dynamic object
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary(object @object)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      FillDictionary(dictionary, @object);
      return dictionary;
    }
    /// <summary>
    /// returns the value of the property identified by propertyName
    /// this method is very slow
    /// </summary>
    /// <param name="object"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object GetPropertyValue(object @object, string propertyName)
    {
      // convert @object to dynamic object
      if (!(@object is IDynamicMetaObjectProvider))
      {
        dynamic dynamicObject = @object;
        @object = dynamicObject;
      }

      var binder = Binder.GetMember(CSharpBinderFlags.None, propertyName, @object.GetType(),
      new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
      var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
      return callsite.Target(callsite, @object);
    }

  }
}
