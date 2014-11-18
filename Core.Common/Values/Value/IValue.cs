
using System;
using System.Threading.Tasks;

namespace Core.Values
{

 

  public interface IValue : ISource, ISink
  {
    IValueInfo ValueInfo{ get; }
    new object Value { get; set; }
  }
  public interface IValue<T> : IValue
  {
    new T Value { get; set; }
  }
}
