using System;
using System.ComponentModel;

namespace Core.Values
{
  public interface ISinkInfo : IConnectorInfo
  {
    bool IsWriteable { get; }
  }
}
