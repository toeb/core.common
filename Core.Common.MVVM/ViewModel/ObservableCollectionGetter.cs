using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Core.Common.Reflect;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Core.Common.MVVM
{
  public class ObservableCollectionGetter : DelegatingGetter
  {
    private PropertySetterDelegate setter;
    public ObservableCollectionGetter(PropertyGetterDelegate inner, PropertySetterDelegate setter) : base(inner) { this.setter = setter; }
    protected override bool Get(object @object, string key, Type type, out object value)
    {
      var success = Inner(@object, key, type, out value);
      if (success && value != null) return true;
      if (!type.IsGenericCollection()) return success;
  
      var itemType = type.GetGenericEnumerableElementType();
      var result = Core.Common.Reflect.Reflection.CreateObservableCollection(itemType);
      setter(@object, key, type, result);
  
      value = result;
      return true;
  
  
    }
  }
}
