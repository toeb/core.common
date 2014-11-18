using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Converters
{
  public class ConversionErrorException : Exception
  {
    private string msg;

    public ConversionErrorException(string msg):base(msg)
    {
      // TODO: Complete member initialization
      this.msg = msg;
    }
  }
}
