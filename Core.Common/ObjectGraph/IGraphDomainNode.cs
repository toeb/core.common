using Core.Values;

namespace Core.ObjectGraph
{
  public interface IGraphDomainNode : IConnectable
  {
    IGraphDomain Domain { get; }
  }
}
