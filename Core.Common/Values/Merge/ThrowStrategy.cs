using Core.Merge;
using System;

namespace Core.Values
{
  public class ThrowStrategy : IMergeStrategy
  {

    public bool CanMerge(object a, object b)
    {
      return true;
    }

    public object Merge(object a, object b)
    {
      throw new Exception("Merge FAiled");
    }
  }
}
