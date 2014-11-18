using Core.Values;

namespace Core.Values
{
  public class AssignableMergeStrategy : ExactTypeMergeStrategy
  {
    public override bool CanMerge(ISource a, ISink b)
    {
      return b.SinkInfo.ValueType.IsAssignableFrom(a.SourceInfo.ValueType);
    }
  }
}
