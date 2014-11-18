using System;

namespace Core.Modules.Entities
{
  public class DataDescription
  {
    public DataDescription() { RequiredType = typeof(object); }
    public DataDescription(Type type)
    {
      Contract = type.FullName;
      RequiredType = type;
    }
    public string Contract { get; set; }
    public Type RequiredType { get; set; }

  }
}
