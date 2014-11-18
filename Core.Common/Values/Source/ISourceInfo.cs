using System;
using System.ComponentModel;

namespace Core.Values
{
  public interface ISourceInfo : IConnectorInfo
  {
    bool IsReadable { get; }
  }
}
