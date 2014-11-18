using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;

namespace Core
{
  /**
   * <summary> Connects two collections by INotifyColelctionChanged events
   * 					 </summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="TSource"> Type of the source.</typeparam>
   * <typeparam name="TSink">   Type of the sink.</typeparam>
   */
  public class CollectionConnector<TSource,TSink>
  {
    /// <summary> source collection </summary>
    private IEnumerable<TSource> _sourceCollection;
    /// <summary> Collection of sinks </summary>
    private ICollection<TSink> _sinkCollection;
    /// <summary> The converter </summary>
    private Func<TSource, TSink> _converter;
    /// <summary> The equals operation </summary>
    private Func<TSource, TSink, bool> _equals;
    /// <summary> A filter  </summary>
    private Func<TSource, bool> _filter;

    /**
     * <summary> Constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="source">    Source for the.</param>
     * <param name="sink">      The sink.</param>
     * <param name="equals">    The equals.</param>
     * <param name="filter">    A filter specifying the.</param>
     * <param name="converter"> The converter.</param>
     */
    public CollectionConnector(
      IEnumerable<TSource> source, 
      ICollection<TSink> sink,  
      Func<TSource,TSink,bool> equals,
      Func<TSource,bool> filter,
      Func<TSource,TSink> converter )
    {
      Contract.Assume(source != null);
      Contract.Assume(converter != null);
      Contract.Assume(sink != null);
      Contract.Assume(filter != null);
      Contract.Assume(equals!= null);
      _converter = converter;
      _equals = equals;
      _filter = filter;
      _sourceCollection = source;
      _sinkCollection = sink;
    
      ResetCollection();
    
      
      var notifyCollectionChanged = source as INotifyCollectionChanged;
      if (notifyCollectionChanged == null) return;
      notifyCollectionChanged.CollectionChanged += SourceCollectionChanged;
    }

    /**
     * <summary> Source collection changed.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="sender"> Source of the event.</param>
     * <param name="e">      Notify collection changed event information.</param>
     */
    void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          AddToSink(e.NewItems);
          break;
        case NotifyCollectionChangedAction.Remove:
          RemoveFromSink(e.OldItems);
          break;
        case NotifyCollectionChangedAction.Move:
          ResetCollection();
          break;
        case NotifyCollectionChangedAction.Replace:
          ResetCollection();
          break;
        case NotifyCollectionChangedAction.Reset:
          ResetCollection();
          break;
      }
    }

    /**
     * <summary> Adds to the sink.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="iList"> Zero-based index of the list.</param>
     */
    private void AddToSink(System.Collections.IList iList)
    {
      foreach (var item in iList)
      {
        FilterAddToSink(item);
      }
    }

    /**
     * <summary> Removes from sink described by iList.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="iList"> Zero-based index of the list.</param>
     */
    private void RemoveFromSink(System.Collections.IList iList)
    {
      foreach (var item in iList)
      {
        var sinkItem = GetSinkItem(item);
        if (sinkItem == null) continue;
        _sinkCollection.Remove(sinkItem);
      }
    }

    /**
     * <summary> Gets a sink item.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="sourceItem"> Source item.</param>
     *
     * <returns> The sink item.</returns>
     */
    private TSink GetSinkItem(object sourceItem)
    {
      if (!(sourceItem is TSource)) return default(TSink);
      var item = (TSource) sourceItem ;
      var query = (from TSink sinkItem in _sinkCollection where _equals((TSource)item, sinkItem) select sinkItem).FirstOrDefault();
      return query;
    }

    /**
     * <summary> Filter add to sink.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="item"> The item.</param>
     */
    private void FilterAddToSink(object item)
    {
      if (!_filter((TSource)item)) return;
      _sinkCollection.Add(_converter((TSource)item));
    }

    /**
     * <summary> Resets the collection.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    private void ResetCollection()
    {
      _sinkCollection.Clear();
      foreach (var item in _sourceCollection)
      {
        FilterAddToSink(item);
      }
    }
  }
}
