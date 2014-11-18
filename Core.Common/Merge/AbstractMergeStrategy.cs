
namespace Core.Merge
{
  public abstract class AbstractMergeStrategy : IMergeStrategy
  {
    public abstract bool CanMerge(object a, object b);
    public abstract object Merge(object a, object b);
  }
}
