using System;
using System.ComponentModel;

namespace Core.Values
{
  public interface IOneWayBinding
  {
    ISink Sink { get; set; }
    ISource Source { get; set; }
  }
}
