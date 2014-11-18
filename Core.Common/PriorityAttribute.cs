using System;
using System.ComponentModel.Composition;
using System.Reflection;
namespace Core
{
  [MetadataAttribute]
  [AttributeUsage(AttributeTargets.Class)]
  public class PriorityAttribute : System.Attribute, IPriority
  {
    public PriorityAttribute(int value) { Value = value; }
    /// <summary>
    /// Priority Value - higher is better
    /// </summary>
    public int Value
    {
      get;
      set;
    }
  }

  public static class Priority
  {
    public static int GetPriority(Type type)
    {
      var attribute = type.GetCustomAttribute<PriorityAttribute>();
      if (attribute == null) return -1;
      return attribute.Value;
    }
    public static int GetPriority(object @object)
    {
      if (@object == null) return -1;
      return GetPriority(@object.GetType());

    }
  }
}
