using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Core
{




  public class CastingCollection<TSource, TSink> : MappingCollection<TSource, TSink>
    where TSink : class
    where TSource : class
  {

    public CastingCollection(ICollection<TSource> sourceCollection)
      : base(
      sourceCollection, 
      (sink, source) => (sink as TSource) == source, 
      source => source as TSink, 
      sink => sink as TSource)
    {

    }

  }
  /**
   * <summary> Maps a collection<TSourceElement> to a collection<TSinkElement> 
   * 					 using the given Funcs to change from one type to another
   * 					 passes along change events.</summary>
   *
   * <remarks> Tobi, 3/14/2012.</remarks>
   *
   * <typeparam name="TSourceElement"> Type of the source element.</typeparam>
   * <typeparam name="TSinkElement">   Type of the sink element.</typeparam>
   */
  public class MappingCollection<TSourceElement, TSinkElement> : MappingEnumerable<TSourceElement, TSinkElement>,
    ICollection<TSinkElement>, INotifyPropertyChanged
  {
    /// <summary> Source for the </summary>
    private ICollection<TSourceElement> _source;

    /**
     * <summary> Gets the property changing source.</summary>
     *
     * <value> The property changing source.</value>
     */
    private INotifyPropertyChanged PropertyChangingSource
    {
      get
      {
        return _source as INotifyPropertyChanged;
      }
    }

    /**
     * <summary> Gets  the are equal operations.</summary>
     *
     * <value> The are equal.</value>
     */
    public Func<TSinkElement, TSourceElement, bool> AreEqual { get; private set; }

    /**
     * <summary>  Gets the mapping function from source element to sink element..</summary>
     *
     * <value> The map to.</value>
     */
    public Func<TSourceElement, TSinkElement> MapTo { get; private set; }

    /**
     * <summary> Gets the mapping function from sink element to source element.</summary>
     *
     * <value> The map from.</value>
     */
    public Func<TSinkElement, TSourceElement> MapFrom { get; private set; }

    /**
     * <summary> Constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="sourceCollection"> source colection</param>
     * <param name="areEqual">         The are equal operator.</param>
     * <param name="toSink">           to sink mapper.</param>
     * <param name="toSource">         to source mapper.</param>
     */
    public MappingCollection(ICollection<TSourceElement> sourceCollection,
      Func<TSinkElement, TSourceElement, bool> areEqual,
      Func<TSourceElement, TSinkElement> toSink,
      Func<TSinkElement, TSourceElement> toSource)
      : base(sourceCollection, toSink)
    {
      MapFrom = toSource;
      MapTo = toSink;
      AreEqual = areEqual;
      _source = sourceCollection;

      if (PropertyChangingSource != null)
      {
        PropertyChangingSource.PropertyChanged += OnPropertyChanged;
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
    void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
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
     * <param name="item"> The TSinkElement to add.</param>
     */
    public void Add(TSinkElement item)
    {
      var source = MapFrom(item);
      _source.Add(source);
    }

    /**
     * <summary> Clears this object to its blank/initial state.</summary>
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
     * <param name="item"> The TSinkElement to test for containment.</param>
     *
     * <returns> true if the object is in this collection, false if not.</returns>
     */
    public bool Contains(TSinkElement item)
    {
      return _source.Any(source => AreEqual(item, source));
    }

    /**
     * <summary> Copies to an array.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="array">      The array.</param>
     * <param name="arrayIndex"> Zero-based index of the array.</param>
     */
    public void CopyTo(TSinkElement[] array, int arrayIndex)
    {
      foreach (var element in this)
      {
        array[arrayIndex++] = element;
      }
    }

    /**
     * <summary> Gets the number of.</summary>
     *
     * <value> The count.</value>
     */
    public int Count
    {
      get { return _source.Count; }
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
     * <param name="item"> The TSinkElement to remove.</param>
     *
     * <returns> true if it succeeds, false if it fails.</returns>
     */
    public bool Remove(TSinkElement item)
    {
      return _source.Remove(MapFrom(item));
    }

    /// <summary> Event queue for all listeners interested in PropertyChanged events. </summary>
    public event PropertyChangedEventHandler PropertyChanged;
  }
}
