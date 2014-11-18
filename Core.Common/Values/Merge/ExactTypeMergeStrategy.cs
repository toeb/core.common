
namespace Core.Values
{
  public class ExactTypeMergeStrategy : AbstractOneWayMergeStrategy
  {

    public override bool CanMerge(ISource a, ISink b)
    {
      return b.SinkInfo.ValueType == a.SourceInfo.ValueType;
    }

    public override void Merge(ISource a, ISink b)
    {
      b.Consume(a.Value);
    }
  }
}
