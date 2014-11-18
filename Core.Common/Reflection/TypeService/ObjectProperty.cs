using System.Diagnostics;


namespace Core.TypeServices
{
  [DebuggerDisplay("{PropertyName}")]
  public class ObjectProperty : AttributeTarget
  {
    public override string ToString()
    {
      return DeclaringType.FullName + "." + PropertyName;
    }

    public string PropertyName { get; set; }
    public ObjectType PropertyType { get; set; }
    public ObjectType DeclaringType { get; set; }
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public ObjectProperty()
    {

    }
    public ObjectProperty(ObjectType declaringType, System.Reflection.PropertyInfo info)
    {
      DeclaringType = declaringType;

    }
  }
}
