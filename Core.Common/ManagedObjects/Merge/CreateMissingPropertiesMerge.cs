using Core.Merge;
using Core.Values;

namespace Core.ManagedObjects
{
  public class CreateMissingPropertiesMerge : AbstractMergeStrategy<IManagedObject, IExtensibleManagedObject, IExtensibleManagedObject>
  {

    public override bool CanMerge(IManagedObject a, IExtensibleManagedObject b)
    {
      return true;
    }

    public override IExtensibleManagedObject Merge(IManagedObject a, IExtensibleManagedObject b)
    {
      foreach (var property in a.Properties)
      {
        if (b.HasProperty(property.PropertyInfo.Name)) continue;
        b.RequireProperty(property.PropertyInfo, property.ToLazy());
      }
      return b;
    }
  }
}
