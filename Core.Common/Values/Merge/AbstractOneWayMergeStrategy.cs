using Core.Values;

namespace Core.Values
{
  public abstract class AbstractOneWayMergeStrategy : AbstractConnectableMergeStrategy, IOneWayMergeStrategy
  {

    public abstract bool CanMerge(ISource a, ISink b);
    public abstract void Merge(ISource a, ISink b);

    public override bool CanMerge(IConnectable a, IConnectable b)
    {
      var source = a as ISource;
      var sink = b as ISink;
      return CanMerge(source, sink);
    }

    public override void Merge(IConnectable a, IConnectable b)
    {
      var source = a as ISource;
      var sink = b as ISink;
      Merge(source, sink);
    }
  }
}
