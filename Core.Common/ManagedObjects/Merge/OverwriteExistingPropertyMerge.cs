using Core.Merge;
using Core.Values;

namespace Core.ManagedObjects
{
  public class OverwriteExistingPropertyMerge : AbstractMergeStrategy<IManagedObject, IManagedObject, IManagedObject>
  {
    public OverwriteExistingPropertyMerge(IMergeStrategy valueStrategy = null) {
      ValueMergeStrategy = valueStrategy ?? SinkToSourceMergeStrategy.Default;
    }

    public IMergeStrategy ValueMergeStrategy { get; private set; }
    public override bool CanMerge(IManagedObject a, IManagedObject b)
    {
      return true;
    }

    public override IManagedObject Merge(IManagedObject a, IManagedObject b)
    {
      foreach (var source in a.Properties)
      {
        var sink = b.GetPropertyOrNull(source.PropertyInfo.Name);
        if (sink == null) continue;
        if (!ValueMergeStrategy.CanMerge(source, sink)) continue;
        ValueMergeStrategy.Merge(source, sink);
      }
      return b;
    }
  }
}
