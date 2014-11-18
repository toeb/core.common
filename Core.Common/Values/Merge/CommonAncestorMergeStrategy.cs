
namespace Core.Values
{
  public class CommonAncestorMergeStrategy : AssignableMergeStrategy
  {

    public override bool CanMerge(ISource a, ISink b)
    {
      var result =  b.ConnectorInfo is IModifiableConnectorInfo;
      return result;
    }

    public override void Merge(ISource a, ISink b)
    {
      var info = b.ConnectorInfo as IModifiableConnectorInfo;
      var commonAncestor = a.ConnectorInfo.ValueType.GetCommonAncestorWith(b.ConnectorInfo.ValueType);
      info.ValueType = commonAncestor;
      info.OnlyExactType = false;
      base.Merge(a, b);
    }
  }
}
