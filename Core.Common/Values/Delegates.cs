using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  //public delegate object ProduceValueDelegate();
  //public delegate void ConsumeValueDelegate(object value);
  public delegate bool CanProduceDelegate();
  //public delegate bool CanConsumeDelegate(object value);
  public delegate bool CanConsumeValueDelegate<in T>(T value);

  public delegate void SetterDelegate<T>(T value);
  public delegate T GetterDelegate<T>();
  public delegate void ValueProducedDelegate(object sender, object value);
  public delegate void ValueConsumedDelegate(object sender, object value);

  
}
