using Core.Graph;
using Core.ManagedObjects;
using Core.Values;

namespace Core.ObjectGraph
{
  public interface IGraphObject : IGraphDomainNode, IAssignableManagedObject, IValue, IExpandable
  {
  }
}
