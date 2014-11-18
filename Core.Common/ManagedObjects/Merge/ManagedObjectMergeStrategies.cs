using Core.Merge;
using Core.Values;

namespace Core.ManagedObjects
{
  public static class ManagedObjectMergeStrategies
  {
    public static readonly IMergeStrategy CreateMissing = new CreateMissingPropertiesMerge();
    public static readonly IMergeStrategy OverwriteExisting = new OverwriteExistingPropertyMerge(SinkToSourceMergeStrategy.Default);
    public static readonly IMergeStrategy Default = new CompositeMergeStrategy(new[] { CreateMissing, OverwriteExisting });
  }
}
