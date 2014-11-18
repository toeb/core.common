using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Naming
{
  public static class NamingExtensions
  {

    public static string Name(this object o){
      if(o is INamedObject){
        var no = o as INamedObject;
        return no.Name;
      }
      return "";
    }
  
  }
}
