using System;
using System.ComponentModel;

namespace Core.Common
{
  public class SimpleDispatcher : IDispatcher
  {
    private static SimpleDispatcher instance;
    public static SimpleDispatcher Instance { get { return instance ?? (instance = new SimpleDispatcher()); } }
  
    public void Dispatch(Action action)
    {
      action();
    }
  }
}
