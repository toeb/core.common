using Core.ManagedObjects;
using Core.Values;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.ObjectGraph
{

  public class GraphObject : AbstractGraphObject
  {
    public GraphObject(Lazy<object> initialValue, GraphDomain domain) : base(initialValue, domain) { }
    private object value;
    protected override object ProduceValue()
    {
      return value;
    }
    protected override void ConsumeValue(object value)
    {
      this.value = value;
    }

    private IEnumerable<PropertyInfo> GetPropertiesForValue(object value)
    { 
      if (value == null) return new PropertyInfo[0];
      return value.GetType().GetProperties();
    }


    protected override void AddValueProperties(object value)
    {
      foreach (var property in GetPropertiesForValue(value))
      {
        this.PushProperty(value, property, SinkToSourceMergeStrategy.Default);
      }
    }

    protected override void RemoveValueProperties(object value)
    {
      foreach (var property in GetPropertiesForValue(value))
      {
        RemoveProperty(property.Name);
      }
    }
  }



}
