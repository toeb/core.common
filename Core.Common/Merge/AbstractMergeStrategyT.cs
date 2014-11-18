using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Merge
{
  public abstract class AbstractMergeStrategy<TA, TB, TResult> :  IMergeStrategy<TA, TB, TResult>
  {
    public abstract bool CanMerge(TA a, TB b);
    public abstract TResult Merge(TA a, TB b);

    public bool CanMerge(object a, object b)
    {
      if (!(a is TA)) return false;
      if (!(b is TB)) return false;
      return CanMerge((TA)a, (TB)b);
    }

    public object Merge(object a, object b)
    {
      return Merge((TA)a, (TB)b);
    }
  }

}
