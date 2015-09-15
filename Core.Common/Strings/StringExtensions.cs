
using Core.Common.Crypto;
using Core.Strings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Common
{
  public static class StringExtensions{


    /// <summary> A string extension method that query if 'a' contains b while ignoring the case. </summary>
    ///
    /// <remarks> Tobi, 30.03.2012. </remarks>
    ///
    /// <param name="a">  a to act on. </param>
    /// <param name="b">  The b. </param>
    ///
    /// <returns> true if it succeeds, false if it fails. </returns>
    public static bool ContainsIgnoreCase(this string a, string b)
    {
      return a.ToLower().Contains(b.ToLower());
    }

    public static IEnumerable<string> Split(this string str, string separator)
    {
      var parts = str.Split(new string[] { separator }, StringSplitOptions.None);
      for (int i = 0; i < parts.Length; i++)
      {
        yield return parts[i];
        if (parts.Length - 1 == i) continue;
        yield return separator;
      }
    }
    public static IEnumerable<string> Split(this string str, IEnumerable<string> separators)
    {
      var queue = new Queue<string>();
      queue.Enqueue(str);
      foreach (var separator in separators)
      {
        var nextQueue = new Queue<string>();
        while (queue.Count != 0)
        {
          var current = queue.Dequeue();
          foreach (var part in current.Split(separator))
          {
            nextQueue.Enqueue(part);
          }
        }
        queue = nextQueue;
      }
      foreach (var part in queue)
      {
        yield return part;
      }
    }

    /// <summary>
    /// formats a string 
    /// </summary>
    /// <param name="format"></param>
    /// <param name="values"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static string FormatWith(this string format, object values, IFormatProvider provider = null)
    {
      if (provider == null) provider = CultureInfo.CurrentCulture;

      var enumerable = values as IEnumerable<object>;
      if (enumerable != null)
      {
        return string.Format(provider, format, enumerable.ToArray());
      }
      if (values is IDictionary)
      {
        return JamesFormatter.Format(format, values, provider, JamesFormatter.DictionaryEval);
      }
      if (values is IDynamicMetaObjectProvider)
      {
        return JamesFormatter.Format(format, values, provider, JamesFormatter.DynamicEval);
      }
      var res = JamesFormatter.Format(format, values, provider);
      return res;
    }

    /// <summary>
    /// returns the content of the string as a stream
    /// 
    /// </summary>
    /// <param name="self"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static Stream AsStream(this string self, Encoding encoding = null)
    {
      encoding = encoding ?? Encoding.Default;
      var bytes = encoding.GetBytes(self);
      var stream = new MemoryStream(bytes);
      return stream;
    }
    /// <summary>
    /// opesn the string as a reader
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static TextReader AsReader(this string self)
    {
      return new StringReader(self);
    }

    /// <summary>
    /// maps any object to a webquery using reflection
    /// this is a simple method which is not useful for complex types
    /// because there are to many unneeded values.
    /// However this method suffices for small anonymous types 
    /// Keep in mind that this is not a serializer - the property values are only their toString expressions
    /// @todo move this somewhere else 
    /// </summary>
    /// <param name="value">the object</param>
    /// <param name="propertyNames">the names of the properties which are to be included</param>
    /// <returns></returns>
    public static string MapWebQuery(object value, params string[] propertyNames)
    {
      var valueType = value.GetType();
      string result = "";
      foreach (var propertyInfo in valueType.GetProperties().Where(p => p.CanRead))
      {
        if (!propertyNames.Contains(propertyInfo.Name)) continue;
        var propertyValue = propertyInfo.GetValue(value, null);
        var name = propertyInfo.Name;
        result += name + "=" + propertyValue + "&";
      }
      result = "?" + result.Substring(0, result.Length - 1);
      return result;
    }
    /// <summary>
    /// Repeats a string n times
    /// </summary>
    /// <param name="str"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string Repeat(this string str, int n)
    {
      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < n; i++)
      {
        builder.Append(str);
      }
      return builder.ToString();
    }
    /// <summary>
    /// Shortens a string to total length replacing the last 4 characters with " ..."
    /// </summary>
    /// <param name="longText"></param>
    /// <param name="totalLength"></param>
    /// <returns></returns>
    public static string Shorten(this string longText, int totalLength)
    {
      if (totalLength < 3) return "...";
      if (longText.Length <= totalLength) return longText;
      return longText.Substring(0, totalLength - 4) + " ...";
    }

    /**
     * \brief A string extension method that queries if a null or is empty.
     *
     * \param str The str to act on.
     *
     * \return  true if a null or is empty, false if not.
     */
    public static bool IsNullOrEmpty(this string str)
    {
      return string.IsNullOrEmpty(str);
    }

    static Random r = new Random((int)DateTime.Now.Ticks);
    /// <summary>
    /// Returns a random string
    /// </summary>
    /// <returns></returns>
    public static string RandomString()
    {
      return Cryptography.Hash((DateTime.Now.ToString() + r.Next()));
    }

    /// <summary>
    /// Hashes a string with a hash algorithm
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Hash(this string str, string algorithm = "sha256")
    {
      return Cryptography.Hash(str, algorithm);
    }


    /// <summary>
    /// Computest the Levenshtein distance between two strings
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static int LevenshteinDistance(this string s, string t)
    {
      //copied from somewhere (forgot)
      int n = s.Length;
      int m = t.Length;
      int[,] d = new int[n + 1, m + 1];

      // Step 1
      if (n == 0)
      {
        return m;
      }

      if (m == 0)
      {
        return n;
      }

      // Step 2
      for (int i = 0; i <= n; d[i, 0] = i++)
      {
      }

      for (int j = 0; j <= m; d[0, j] = j++)
      {
      }

      // Step 3
      for (int i = 1; i <= n; i++)
      {
        //Step 4
        for (int j = 1; j <= m; j++)
        {
          // Step 5
          int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

          // Step 6
          d[i, j] = Math.Min(
              Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
              d[i - 1, j - 1] + cost);
        }
      }
      // Step 7
      return d[n, m];
    }
  }
}
