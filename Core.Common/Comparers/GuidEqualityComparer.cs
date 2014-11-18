using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

  public class IntEqualityComparer : EqualityComparer<int>
  {

    public override bool Equals(int x, int y)
    {
      return x == y;
    }

    public override int GetHashCode(int obj)
    {
      return obj;
    }
  }



  public class GuidEqualityComparer : EqualityComparer<Guid>
  {

    public override bool Equals(Guid x, Guid y)
    {
      return x.Equals(y);
    }

    public override int GetHashCode(Guid obj)
    {
      return obj.GetHashCode();
    }
  }


}
