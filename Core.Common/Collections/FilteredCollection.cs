using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Core
{


  /**
   * <summary> Filtering Collection filters a collection while still being Observable
   * 					 (if soruce collection implements INotifyCollectionChanged it will pass along these events)</summary>
   *
   * <remarks> Tobi, 3/14/2012.</remarks>
   *
   * <typeparam name="T"> Generic type parameter.</typeparam>
   */
  public class FilteringCollection<T> : FilteredEnumerable<T>,
    ICollection<T>, INotifyPropertyChanged
  {
    /**
     * <summary> Gets the notify property changed source.</summary>
     *
     * <value> The notify property changed source.</value>
     */
    private INotifyPropertyChanged NotifyPropertyChangedSource { get { return _source as INotifyPropertyChanged; } }
    /// <summary> Source for the </summary>
    private ICollection<T> _source;

    /**
     * <summary> Constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="source">    Source collection.</param>
     * <param name="predicate"> The predicate.</param>
     */
    public FilteringCollection(ICollection<T> source, Predicate<T> predicate)
      : base(source, predicate)
    {
      _source = source;
      if (NotifyPropertyChangedSource != null)
      {
        NotifyPropertyChangedSource.PropertyChanged += OnPropertyChanged;
      }
    }

    /**
     * <summary> Executes the property changed action.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="sender"> Source of the event.</param>
     * <param name="e">      Event information to send to registered event handlers.</param>
     */
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "Count":
        case "IsReadOnly":
          if (PropertyChanged != null) PropertyChanged(this, e);
          break;
      }
    }

    /**
     * <summary> Adds item..</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="item"> The T to add.</param>
     */
    public void Add(T item)
    {
      if (!Predicate(item)) return;
      _source.Add(item);
    }

    /**
     * <summary> Clears this colleciton to its blank/initial state.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    public void Clear()
    {
      _source.Clear();
    }

    /**
     * <summary> Query if this object contains the given item.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="item"> The T to test for containment.</param>
     *
     * <returns> true if the object is in this collection, false if not.</returns>
     */
    public bool Contains(T item)
    {
      if (!Predicate(item)) return false;
      return _source.Contains(item);
    }

    /**
     * <summary> Copies colleciton to array.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="array">      The array.</param>
     * <param name="arrayIndex"> Zero-based index of the array.</param>
     */
    public void CopyTo(T[] array, int arrayIndex)
    {
      foreach (var item in this)
      {
        array[arrayIndex++] = item;
      }
    }

    /**
     * <summary> Gets the number of elements.</summary>
     *
     * <value> The count.</value>
     */
    public int Count
    {
      get { return _source.Count((t) => Predicate(t)); }
    }

    /**
     * <summary> Gets a value indicating whether this object is read only.</summary>
     *
     * <value> true if this object is read only, false if not.</value>
     */
    public bool IsReadOnly
    {
      get { return _source.IsReadOnly; }
    }

    /**
     * <summary> Removes the given item.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="item"> The T to remove.</param>
     *
     * <returns> true if it succeeds, false if it fails.</returns>
     */
    public bool Remove(T item)
    {
      if (!Predicate(item)) return false;
      return _source.Remove(item);
    }

    /// <summary> Event queue for all listeners interested in PropertyChanged events. </summary>
    public event PropertyChangedEventHandler PropertyChanged;
  }

}
