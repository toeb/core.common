using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Common.Reflect;
using System.Collections;

namespace Core.Strings
{
  public static class JamesFormatter
  {
    public delegate object EvalDelegate(object source, string expression);



    public static string Format(string format, object source, IFormatProvider provider = null, EvalDelegate Eval = null)
    {
      if (provider == null) provider = CultureInfo.CurrentCulture;
      if (Eval == null) Eval = ReflectionEval;

      if (format == null)
        throw new ArgumentNullException("format");

      List<object> values = new List<object>();
      string rewrittenFormat = Regex.Replace(format,
          @"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
          delegate(Match m)
          {
            Group startGroup = m.Groups["start"];
            Group propertyGroup = m.Groups["property"];
            Group formatGroup = m.Groups["format"];
            Group endGroup = m.Groups["end"];

            values.Add(Eval(source, propertyGroup.Value));

            int openings = startGroup.Captures.Count;
            int closings = endGroup.Captures.Count;

            return openings > closings || openings % 2 == 0
                 ? m.Value
                 : new string('{', openings) + (values.Count - 1) + formatGroup.Value
                   + new string('}', closings);
          },
          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

      return string.Format(provider, rewrittenFormat, values.ToArray());
    }
    /// <summary>
    /// slooooooooooooow
    /// </summary>
    /// <param name="o"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static object DynamicEval(object o, string expression)
    {
      dynamic @object = o;
      return DynamicObject.GetPropertyValue(@object, expression);
    }
    public static object DictionaryEval(object source, string expression)
    {
      var dict = source as IDictionary;
      if (dict == null) throw new ArgumentException("value source must be a dictionary");
      return dict[expression];
    }
    public static object ReflectionEval(object source, string expression)
    {

      var val = source.ReflectPropertyValue(expression, true);
      if (val == null)
      {
        int i;
        if (!int.TryParse(expression, out i)) return null;
        val = source.ReflectPropertyValueByIndex(i);
      }
      return val;
    }


  }
}
