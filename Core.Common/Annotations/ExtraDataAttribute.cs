using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Annotations
{
  /// <summary>
  /// Indicates that extra data can be stored in class, method or field
  /// assuming the following
  /// if applied to a class:
  /// *  class Inherits from ICollection<<KeyValuePair<string,object>>
  /// if applied to a property
  /// * property type is assignable to ICollection<<KeyValuePair<string,object>> (IDictionary<string,object> e.g.)
  /// if applied to a this supports write only
  /// * method accepts one argument of type KeyValuePair<string,object>
  /// 
  /// </summary>
  public class ExtraDataAttribute : System.Attribute
  {

  }
}
