using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{

  /// <summary>
  /// base class for connectable values
  /// </summary>
  public interface IConnectable : INode<IConnectable, IConnection>
  {

  }



}
