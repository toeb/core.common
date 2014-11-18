using System;

namespace Core.Values
{


  /// <summary>
  /// 
  /// </summary>
  public interface IConnectorInfo
  {
    Type ValueType { get; }
    /// <summary>
    /// specifies of connector only accepts the exact type specified an no others (subtypes)
    /// </summary>
    bool OnlyExactType { get; }
    bool IsValidValue(object value);
    bool IsValidValueType(Type type);
  }

}
