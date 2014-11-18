using System.Reflection;

namespace Core.Resources
{
  public class StringResource : ManagedResource, IStringResource
  {
    public StringResource(string key, string value, Assembly assembly)
      : base(key, assembly)
    {
      Value = value;
    }
    public string Value
    {
      get;
      private set;
    }
  }
}
