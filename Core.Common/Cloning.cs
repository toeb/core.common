using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class CloningExtensions
  {
    public static T DeepClone<T>(this T original)
    {
      if ((object)original == null) return default(T);
      return GeorgeCloney.CloneExtension.DeepClone(original);
    }

    private static MethodInfo memberwiseClone;
    private static object[] empty = new object[0]; 
    public static T ShallowClone<T>(this T original)
    {
      if((object)original == null)return default (T);
      if (memberwiseClone == null) memberwiseClone = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic);
      var result = (T)memberwiseClone.Invoke(original,empty);
      return result;      
    }
  }
}

