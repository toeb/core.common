using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Core
{
  /**
 * <summary> An Enumerable that filters a source enumerable
 * 					 passing along CollectionChange events
 * 					 Usefull for UI DataBinding</summary>
 *
 * <remarks> Tobi, 3/15/2012.</remarks>
 *
 * <typeparam name="T"> Generic type parameter.</typeparam>
 */
  public class EnumerableSelection<TResult, TSource> : IEnumerable<TResult>, INotifyCollectionChanged
  {
    /**
     * <summary> Gets the predicate to filter by.</summary>
     *
     * <value> The predicate.</value>
     */
    public Predicate<TSource> Predicate { get; private set; }

    public Func<TSource, TResult> Selector { get; private set; }

    /**
     * <summary> Gets the notifying source.</summary>
     *
     * <value> The notifying source.</value>
     */
    private INotifyCollectionChanged NotifyingSource { get { return _source as INotifyCollectionChanged; } }

    /// <summary> Source for this Filtering Enumerable </summary>
    private IEnumerable<TSource> _source;

    /**
     * <summary> Constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="source">    Source enumerable.</param>
     * <param name="predicate"> The predicate by which source is filtered.</param>
     */
    public EnumerableSelection(IEnumerable<TSource> source, Func<TSource, TResult> selector, Predicate<TSource> predicate=null)
    {
      if (predicate == null) predicate = o => true;
      Predicate = predicate;
      Selector = selector;
      _source = source;
      //hookup event if source implements INotifyCollectionChanged
      if (NotifyingSource != null)
      {
        NotifyingSource.CollectionChanged += OnCollectionChange;
      }
    }

    /**
     * <summary> Executes the collection change action.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="sender"> Source of the event.</param>
     * <param name="e">      Event information to send to registered event handlers.</param>
     */
    private void OnCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
    {
      var oldItems = new List<object>();
      var newItems = new List<object>();
      if (e.OldItems != null)
      {
        foreach (var item in e.OldItems)
        {
          if (!(item is TSource)) continue;
          if (Predicate((TSource)item)) oldItems.Add(Selector((TSource)item));
        }
      }
      if (e.NewItems != null)
      {
        foreach (var item in e.NewItems)
        {
          var t = typeof(TSource);
          if (!(item is TSource)) continue;
          if (Predicate((TSource)item)) newItems.Add(Selector((TSource)item));
        }
      }
      //if (e.Action != NotifyCollectionChangedAction.Replace) return;

      NotifyCollectionChangedEventArgs args = null;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          if (newItems.Count != 1) return;
          args = new NotifyCollectionChangedEventArgs(e.Action, newItems.First());
          break;
      }
      if (args == null) return;
      if (CollectionChanged != null) CollectionChanged(this, args);
    }

    /**
     * <summary> Gets the enumerator which filters the source 
     * 					 enumerables items by predicate.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <returns> The enumerator.</returns>
     */
    public IEnumerator<TResult> GetEnumerator()
    {
      foreach (var item in _source)
      {
        if (Predicate(item)) yield return Selector(item);
      }
    }

    /**
     * <summary> Gets the enumerator.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <returns> The enumerator.</returns>
     */
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary> Event queue for all listeners interested in CollectionChanged events. </summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;
  }
}
