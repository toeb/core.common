using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Core
{
  /**
   * <summary> Mapping enumerable. Maps a specific enumerable to another
   * 					 useful for wrapping items of an enumerable</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="TSourceElement"> Type of the source element.</typeparam>
   * <typeparam name="TSinkElement">   Type of the sink element.</typeparam>
   */
  public class MappingEnumerable<TSourceElement, TSinkElement> : IEnumerable<TSinkElement>, INotifyCollectionChanged
  {
    /**
     * <summary> Gets  the map.</summary>
     *
     * <value> The map.</value>
     */
    public Func<TSourceElement, TSinkElement> Map { get; private set; }
    /// <summary> source enumerable </summary>
    IEnumerable<TSourceElement> _source;

    public INotifyCollectionChanged NotifyingCollection
    {
      get
      {
        return _source as INotifyCollectionChanged;
      }
    }

    public MappingEnumerable(IEnumerable<TSourceElement> source, Func<TSourceElement, TSinkElement> MapSourceToSink)
    {
      _source = source;
      Map = MapSourceToSink;
      if (NotifyingCollection != null)
      {
        NotifyingCollection.CollectionChanged += OnCollectionChange;
      }
    }

    /**
     * <summary> Gets the enumerator.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <returns> The enumerator.</returns>
     */
    public IEnumerator<TSinkElement> GetEnumerator()
    {
      foreach (var element in _source)
      {
        yield return Map(element);
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
    void OnCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
    {
      var oldItems = new List<TSinkElement>();
      var newItems = new List<TSinkElement>();
      if (e.OldItems!=null)
      {
        foreach (var item in e.OldItems)
        {
          oldItems.Add(Map((TSourceElement)item));
        }
      }
      if (e.NewItems!=null)
      {
        foreach (var item in e.NewItems)
        {
          newItems.Add(Map((TSourceElement)item));
        }
      }
      NotifyCollectionChangedEventArgs args;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          args = new NotifyCollectionChangedEventArgs(e.Action, newItems);
          break;
        case NotifyCollectionChangedAction.Remove:
          args = new NotifyCollectionChangedEventArgs(e.Action, oldItems);
          break;
        case NotifyCollectionChangedAction.Replace:
          args = new NotifyCollectionChangedEventArgs(e.Action, newItems, oldItems);
          break;
        case NotifyCollectionChangedAction.Reset:
          args = new NotifyCollectionChangedEventArgs(e.Action, newItems,oldItems);
          break;
        default:
          return;
      }
      
      if (CollectionChanged != null) CollectionChanged(this, args);
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
