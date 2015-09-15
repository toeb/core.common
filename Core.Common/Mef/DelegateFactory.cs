using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core
{
  public class DelegateFactory<T> : IFactory<T>
  {
  
    public Type ProductType
    {
      get { return typeof(T); }
    }
    public DelegateFactory(Func<T> create)
    {
      this.create = create;
    }
    public T Create()
    {
      return create();
    }
  
    public Func<T> create { get; set; }
  }
}
