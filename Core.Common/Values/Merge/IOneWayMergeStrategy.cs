using Core.Values;

namespace Core.Values
{
  public interface IOneWayMergeStrategy : IConnectableMergeStrategy
  {
    bool CanMerge(ISource a, ISink b);
    void Merge(ISource a, ISink b);
  }
}
