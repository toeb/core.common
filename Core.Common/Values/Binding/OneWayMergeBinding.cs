using Core.Merge;
using Core.Values;

namespace Core.Values
{
  public class OneWayMergeBinding : AbstractOneWayBinding
  {
    private IMergeStrategy strategy;
    public OneWayMergeBinding(IMergeStrategy strategy)
    {
      Strategy = strategy;
    }
    public override void Sync()
    {
      if (!Strategy.CanMerge(Source, Sink)) throw new PropertyMergeException();
      Strategy.Merge(Source, Sink);
    }

    public IMergeStrategy Strategy { get { return strategy; } set { ChangeIfDifferentAndCallback(ref strategy, value, StrategyChanging, StrategyChanged, StrategyName); } }
    private static readonly string StrategyName = "Strategy";
    protected virtual void StrategyChanged(IMergeStrategy oldValue, IMergeStrategy newValue) { }
    protected virtual void StrategyChanging(IMergeStrategy oldValue, IMergeStrategy newValue) { }

  }
}
