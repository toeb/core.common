using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.Graphs
{
  public class Marker<T>
  {
    private readonly Dictionary<object, T> dict = new Dictionary<object, T>();
    public T this[object it]
    {
      get
      {
        if (!dict.ContainsKey(it)) return default(T);
        return dict[it];
      }
      set
      {
        dict[it] = value;
      }
    }


  }


  /// <summary>
  /// class used to mark objets ( used for algorithms)
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Marker : Marker<bool>
  {

  }

}
