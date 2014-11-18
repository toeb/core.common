using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{

  public interface ISink<T> : ISink
  {
    new T Value { set; }
  }
}
