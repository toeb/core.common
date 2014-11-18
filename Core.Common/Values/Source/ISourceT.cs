using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  public interface ISource<T> : ISource
  {
    new T Value { get; }
  }
}
