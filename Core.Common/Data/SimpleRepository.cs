using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Core.Common.Data
{
  public class SimpleRepository<T> : IRepository<T> where T : class
  {
    public SimpleRepository(IDataContext context) { this.DataContext = context; }
    public IDataContext DataContext { get; private set; }
  
    public void Create(T entity)
    {
      
      DataContext.Set<T>().Add(entity);
    }
  
    public void Delete(Func<T, bool> predicate)
    {
  
      var existing = DataContext.Set<T>().Single(predicate);
      DataContext.Set<T>().Remove(existing);
    }
  
    public void Update(Func<T, bool> predicate, T entity)
    {
      var existing = Get().Single(predicate);
      Delete(predicate);
      Create(entity);
    }
  
    public IEnumerable<T> Get()
    {
      return DataContext.Set<T>();
    }
  
  }
}
