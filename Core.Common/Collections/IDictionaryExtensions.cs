using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public enum DictionaryMergeStrategy
  {
    LeftWins,
    RightWins,
  }
  public static class IDictionaryExtensions
  {
    public static void OverwriteValuesWith<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
      foreach (var kv in values)
      {
        target[kv.Key] = kv.Value;
      }
    }


    public static IDictionary<TKey, TValue> Merge<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> left, IEnumerable<KeyValuePair<TKey, TValue>> right, DictionaryMergeStrategy strategy = DictionaryMergeStrategy.LeftWins)
    {
      var dict = new Dictionary<TKey, TValue>();
      Merge(dict, left, right, strategy);
      return dict;
    }

    public static void MergeInplace<TKey, TValue>(this IDictionary<TKey, TValue> left, IEnumerable<KeyValuePair<TKey, TValue>> right, DictionaryMergeStrategy strategy)
    {
      Merge(left, left, right, strategy);
    }

    public static void Merge<TKey, TValue>(IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> left, IEnumerable<KeyValuePair<TKey, TValue>> right, DictionaryMergeStrategy strategy = DictionaryMergeStrategy.LeftWins)
    {
      IEnumerable<KeyValuePair<TKey, TValue>> source;
      IEnumerable<KeyValuePair<TKey, TValue>> target;

      switch (strategy)
      {
        case DictionaryMergeStrategy.LeftWins:
          target = right.ToArray();
          source = left.ToArray();
          break;
        case DictionaryMergeStrategy.RightWins:
          target = left.ToArray();
          source = right.ToArray();
          break;
        default:
          return;
      };

      dict.OverwriteValuesWith(target);
      dict.OverwriteValuesWith(source);
    }

    public static IDictionary<TKey, TValue> Copy<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
      var dict = new Dictionary<TKey, TValue>();
      dict.OverwriteValuesWith(values);
      return dict;
    }
  }
}
