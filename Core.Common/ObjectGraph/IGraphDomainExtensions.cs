using Core.Merge;
using System.Collections.Generic;
using System.Linq;

namespace Core.ObjectGraph
{
  public static class IGraphDomainExtensions
  {
    public static IEnumerable<object> GetValues(this IGraphDomain domain)
    {
      return domain.Nodes.OfType<IGraphObject>().Select(o => o.Value);
    }

    public static IGraphObject Create(this IGraphDomain domain)
    {
      return domain.Create(null);
    }
  }
}
