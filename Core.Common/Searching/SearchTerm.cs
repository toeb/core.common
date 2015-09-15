using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common
{
  public struct SearchTerm
  {
    public object @object;
    public string[] keywords;
    public static implicit operator SearchTerm(string @string)
    {  
      return new SearchTerm() { keywords = new string[]{@string}, @object = @string };
    }
  
  }
}
