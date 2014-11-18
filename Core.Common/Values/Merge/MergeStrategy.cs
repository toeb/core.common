
using Core.Merge;
namespace Core.Values
{
  public static class SinkToSourceMergeStrategy
  {
    
    public static readonly IMergeStrategy Throw = new ThrowStrategy();
    public static readonly IMergeStrategy ExactType = new ExactTypeMergeStrategy();
    public static readonly IMergeStrategy Assignable = new AssignableMergeStrategy();
    public static readonly IMergeStrategy CommonAncestor = new CommonAncestorMergeStrategy();
    public static readonly IMergeStrategy Default = new CompositeMergeStrategy(new[] { ExactType, Assignable, CommonAncestor });
    public static readonly IMergeStrategy ThrowOnUnassignable = new CompositeMergeStrategy(new[] { ExactType, Assignable, Throw });

  }
}
