using Core.Merge;

namespace Core.Values
{
  public abstract class AbstractConnectableMergeStrategy : AbstractMergeStrategy, IConnectableMergeStrategy
  {
    public abstract bool CanMerge(IConnectable a, IConnectable b);
    public abstract void Merge(IConnectable a, IConnectable b);

    public override bool CanMerge(object a, object b)
    {
      var ca = a as IConnectable;
      var cb = b as IConnectable;
      if (ca == null && cb == null) return false;
      return CanMerge(ca, cb);
    }

    public override object Merge(object a, object b)
    {
      var ca = a as IConnectable;
      var cb = b as IConnectable;
      Merge(ca, cb);
      return a;
    }
  }
}
