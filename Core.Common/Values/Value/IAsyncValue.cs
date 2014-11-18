using System;
using System.Threading.Tasks;

namespace Core.Values
{
  public interface IAsyncValue : IValue, IAsyncSource, IAsyncSink
  {

  }
}
