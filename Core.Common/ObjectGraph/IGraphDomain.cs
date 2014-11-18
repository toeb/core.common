using Core.Graph.Directed;
using Core.Values;

namespace Core.ObjectGraph
{
  public interface IGraphDomain : 
    IGraph<IConnectable, IConnection, IGraphDomain>
  {
    IGraphObject Get(object value);
    IGraphObject Create(object value);
    IGraphObject Require(object value);
  }
}
