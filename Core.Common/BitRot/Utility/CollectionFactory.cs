using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Core.Collections;

namespace Core
{
  /**
   * <summary> Collection factory.</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   */
  public class CollectionFactory
  {
    /// <summary> The collections created in this factory </summary>
    private static List<ICollection> _collections = new List<ICollection>();
    /// <summary> The dictionaries created in this factory </summary>
    private static List<IDictionary> _dictionaries = new List<IDictionary>();

    /**
     * <summary> Creates a collection.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <typeparam name="T"> Generic type parameter.</typeparam>
     * <param name="observable"> (optional) the collection implements INotifyCollectionChanged when true.</param>
     *
     * <returns> A list of.</returns>
     */
    public static ICollection<T> CreateCollection<T>(bool observable = false)
    {
      ICollection ret;
      if (!observable)

        ret = new List<T>(); //new DispatchingObservableCollection<T>();
      else
      {
        ret = new DispatchingObservableCollection<T>();
      }
      _collections.Add(ret);
      return (ICollection<T>)ret ;
    }

    /**
     * <summary> Creates a dictionary.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <typeparam name="TKey">   Type of the key.</typeparam>
     * <typeparam name="TValue"> Type of the value.</typeparam>
     *
     * <returns> .</returns>
     */
    public static IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
    {
      var ret = new Dictionary<TKey, TValue>();
      _dictionaries.Add(ret);
      return ret;
    }
  }
}
