using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core
{
  public interface IFactory<out T>
  {
    Type ProductType { get ;  }
    T Create();
  }
}
