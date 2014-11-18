using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{
  public interface IFormat
  {
    string Name { get; }
    Type Type { get; }
    bool IsAssignableFrom(IFormat other);
  }



  public static class IFormatExtensions
  {
    public static bool IsAssignableTo(this IFormat @this, IFormat other)
    {
      return other.IsAssignableFrom(@this);
    }
  }
}
