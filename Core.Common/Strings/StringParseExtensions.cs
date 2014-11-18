using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Reflection;
using System.Linq.Expressions;
using System.IO;
using System.Globalization;
using System.Collections;
using Core.Strings;
using System.Dynamic;

namespace Core.Strings
{
  public static class StringParseExtensions
  {

    public static T? AsNullable<T>(this T t) where T : struct
    {
      return new Nullable<T>(t);
    }
    public static int? ParseInt(this string code)
    {

      int result;
      if (!int.TryParse(code, out result)) return null;

      return result;
    }
  }
}
