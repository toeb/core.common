using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
namespace Core.Graph
{
  public class TraversalContext<T>
  {
    public T Current { get; set; }
    public T CurrentPredecessor { get; set; }
    public TraversalContext<T> CurrentPreceedingContext { get; set; }
    public IEnumerable<TraversalContext<T>> ContextPath { get; set; }
    public IEnumerable<T> Path { get; set; }
    public int Depth { get; set; }


    public static TraversalContext<T> Create(T currentElement, TraversalContext<T> preceedingContext)
    {
      var result = new TraversalContext<T>();
      if (preceedingContext == null)
      {
        result.ContextPath = result.MakeArray();
        result.Depth = 0;
        result.Path = currentElement.MakeArray();
        result.CurrentPredecessor = default(T);
      }
      else
      {
        result.Depth = preceedingContext.Depth + 1;
        result.ContextPath = preceedingContext.ContextPath.Concat(result);
        result.Path = preceedingContext.Path.Concat(currentElement);
        result.CurrentPredecessor = preceedingContext.Current;
      }
      result.Current = currentElement;
      result.CurrentPreceedingContext = preceedingContext;
      return result;
    }

  }
}
