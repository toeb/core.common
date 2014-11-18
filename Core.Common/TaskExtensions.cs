using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class TaskExtensions
  {
    /// <summary>
    /// waits for a task to finish, returning the result or throwing a exception (AggregateException) on failure
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static TResult Await<TResult>(this Task<TResult> task)
    {
      task.Wait();
      return task.Result;
    }
    /// <summary>
    /// waits for a resultless task to finish, throwing an exception on error
    /// </summary>
    /// <param name="task"></param>
    public static void Await(this Task task)
    {
      task.Wait();
    }
  }
}
