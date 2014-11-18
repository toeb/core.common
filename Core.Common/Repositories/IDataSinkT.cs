using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public interface IDataSink<in T> : IDataSink
  {
    void Create(T item);
    void Update(T item);
    void Delete(T item);
  }

  public interface IAsyncDataSink<in T> : IAsyncDataSink, IDataSink<T>
  {
    Task CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(T item);
  }

  public static class DataSinkExtensions
  {
    public static T CreateAndReturn<T>(this IDataSink<T> sink, T item)
    {
      sink.Create(item);
      return item;
    }
    public static T UpdateAndReturn<T>(this IDataSink<T> sink, T item)
    {
      sink.Update(item);
      return item;
    }
  }
}
