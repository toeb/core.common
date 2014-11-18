using System;
using System.ComponentModel;

namespace Core.Values
{
  public interface IModifiableConnectorInfo : IConnectorInfo, INotifyPropertyChanged
  {
    new bool OnlyExactType { get; set; }
    new Type ValueType { get; set; }
  }
}
