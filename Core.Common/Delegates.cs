using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public delegate void NotifyDelegate();
  public delegate void ValueChangeDelegate(object sender, ValueChangeEventArgs args);
  public delegate TOut TransformValueDelegate<in TIn, out TOut>(TIn input);
  public delegate void OnValueChangeDelegate<T>(T oldValue, T newValue);
  public delegate T ProduceValueDelegate<out T>();
  public delegate void ConsumeValueDelegate<in T>(T value);

  public delegate void ElementsAdded<T>(IEnumerable<T> addedElements);
  public delegate void ElementsRemoved<T>(IEnumerable<T> removedElements);
  public delegate void ElementAdded<T>(T addedElement);
  public delegate void ElementRemoved<T>(T removedElement);

  public delegate void ToStreamDelegate<in T>(T input, Stream stream);
  public delegate T FromStreamDelegate<out T>(Stream input);
}
