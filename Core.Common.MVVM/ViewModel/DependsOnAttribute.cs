using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;

namespace Core.Common.MVVM
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
  public class DependsOnAttribute : System.Attribute
  {
    public DependsOnAttribute(string propertyName)
    {
      this.PropertyName = propertyName;
    }
  
    public string PropertyName { get; set; }
  }
}
