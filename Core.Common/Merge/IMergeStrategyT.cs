using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Merge
{

  public interface IMergeStrategy<TA, TB, TResult> : IMergeStrategy
  {
    bool CanMerge(TA a, TB b);
    TResult Merge(TA a, TB b);
  }
}
