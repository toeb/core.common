using Core.ManagedObjects;

namespace Core.ObjectGraph
{
  public interface IGraphProperty : IGraphDomainNode, IManagedProperty
  {
    IGraphObject GraphObject { get; }
  }
}
