
namespace Core.Merge
{
  public interface IMergeStrategy
  {
    bool CanMerge(object a, object b);
    object Merge(object a, object b);
  }
}
