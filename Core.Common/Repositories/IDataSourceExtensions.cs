
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public static class IDataSourceExtensions
  {
    public static IQueryable<T> Where<T>(this IRepository<T> _this, Expression<Func<T, bool>> predicate)
    {
      return _this.Read().Where(predicate);
    }
    public static T Find<T>(this IRepository<T> _this, Expression<Func<T, bool>> predicate)
    {
      return _this.Where(predicate).SingleOrDefault();
    }
    public static T Find<T>(this IRepository<T> _this, object query)
    {
      return _this.Where(query).SingleOrDefault();
    }
    public static IQueryable<T> Where<T>(this IRepository<T> _this, object query)
    {
      var type = query.GetType();
      var objectType = (typeof(T));
      var properties = type.GetProperties();
      var result = _this.Read();
      foreach (var queryProperty in properties)
      {        
        var name = queryProperty.Name;
        var value = queryProperty.GetValue(query);
        var objectProperty = objectType.GetProperty(name);
        if (objectProperty == null) return result.Where(it => false);
        result = result.Where(it => objectProperty.GetValue(it).Equals(value));
      }
      return result;
    }

  }
}
