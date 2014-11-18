using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  /// <summary>
  /// base interface for a connection
  /// </summary>
  public interface IConnection : IEdge<IConnectable, IConnection>
  {

  }
}
