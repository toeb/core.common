using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Data
{
  public interface IEntry
  {
    object Value { get; }
    DateTime ChangeDate { get; }
    EntityState State { get; set; }
    object Id { get; }
    object Hash { get; }
  }


  public interface IDataContext
  {
    ICollection<T> Set<T>() where T : class;
    event DataChangedEventHandler DataChanged;
    IEntry Entry(object entity);
    Task SaveAsync();
    Task RefreshAsync();
  }
  public static class TaskExtensions
  {
    public static void WaitToFinish(this Task task)
    {
      task.Wait();      
    }

  }
  public static class IDataContextExtensions
  {
    public static void Save(this IDataContext context)
    {
      var task = context.SaveAsync();
      task.WaitToFinish();
      
    }
    public static void Refresh(this IDataContext context)
    {
      context.RefreshAsync().Wait();

    }
  }
}
