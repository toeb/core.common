using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Core.ManagedObjects
{
  [DebuggerDisplay("conn{Tail}->{Head}")]
  public class AbstractObjectPropertyConnection : AbstractConnection, IObjectPropertyConnection
  {

  }
}
