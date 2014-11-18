using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Equality
  {

    public static bool Check(object a, object b)
    {
      if (object.ReferenceEquals(null, a))
        return object.ReferenceEquals(null, b);
      return a.Equals(b);
    }
  }
}
