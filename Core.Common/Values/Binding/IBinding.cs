using System;
using System.ComponentModel;

namespace Core.Values
{
  public interface IBinding
  {
    BindingState State { get; }
    void Enable();
    void Disable();
  }
}
