using System;

namespace Core.Modules.Entities
{
  public class DataSinkDescription : DataDescription
  {
    public DataSinkDescription(){}
    public DataSinkDescription(Type type) : base(type) { }
    public Type DataSinkType { get; set; }
  }
}
