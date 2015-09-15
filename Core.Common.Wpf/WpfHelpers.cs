using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Core.Common.Wpf
{
  public static class WpfHelpers
  {
    
    public static IEnumerable<T> Ancestors<T>(this DependencyObject current) where T : DependencyObject
    {
      return current.Ancestors().Where(d => d is T).Cast<T>();
    }
    public static IEnumerable<DependencyObject> Ancestors(this DependencyObject current)
    {
      while (true)
      {
        var next = VisualTreeHelper.GetParent(current);
        if (next == null)
        {
          yield break;
        }
        yield return next;
        current = next;
        
      }
  
    }
  }
}
