
namespace Core.ObjectGraph
{
  public class GraphProperty : AbstractGraphProperty
  {
    public GraphProperty(string name, IGraphObject graphObjectValue, GraphObject @object, GraphDomain domain)
      : base(name, graphObjectValue, @object, domain) { }
  }

}
