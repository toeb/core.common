using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class GuidExtensions
  {
    public static bool IsEmpty(this Guid id)
    {
      return id == Guid.Empty;
    }
  }
}
