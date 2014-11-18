using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  public interface IManagedProperty<T> : IManagedProperty, IValue<T>
  {
  }
}
