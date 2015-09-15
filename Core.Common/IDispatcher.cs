using System;
using System.ComponentModel;

namespace Core.Common
{
  /// <summary>
  /// A dispatcher is used to execute a task on a specifiec thread.
  /// It acts like a action sink
  /// </summary>
  public interface IDispatcher
  {
    void Dispatch(Action action);
  }

}
