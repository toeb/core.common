
using Core.Merge;
namespace Core.ManagedObjects
{
  public interface IAssignableManagedObject : IManagedObject
  {
    void PushProperty(IManagedProperty property, IMergeStrategy strategy);
    void PullProperty(IManagedProperty property, IMergeStrategy strategy);
    //IBinding Bind(IManagedProperty property);
  }
}
