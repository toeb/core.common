using Core.Merge;
using Core.Values;

namespace Core.Values
{
  public interface IConnectableMergeStrategy : IMergeStrategy
  {
    bool CanMerge(IConnectable a, IConnectable b);
    void Merge(IConnectable a, IConnectable b);
  }
}
