using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Collections
{
  /**
   * <summary> Class.</summary>
   *
   * <remarks> Tobi, 3/16/2012.</remarks>
   *
   * <typeparam name="T"> Generic type parameter.</typeparam>
   */
  public class ObservableStack<T> : NotifyPropertyChangedBase
  {
    /**
     * <summary> Pushes an object onto this stack.</summary>
     *
     * <remarks> Tobi, 3/16/2012.</remarks>
     *
     * <param name="element"> The T to push.</param>
     */
    public void Push(T element) { _data.Add(element); CheckEmpty(); }

    /**
     * <summary> Returns the top-of-stack object without removing it.</summary>
     *
     * <remarks> Tobi, 3/16/2012.</remarks>
     *
     * <returns> The current top-of-stack object.</returns>
     */
    public T Peek() { return _data.Last(); }

    /**
     * <summary> Removes and returns the top-of-stack object.</summary>
     *
     * <remarks> Tobi, 3/16/2012.</remarks>
     *
     * <returns> The previous top-of-stack object.</returns>
     */
    public T Pop() { var element = Peek(); _data.Remove(element); CheckEmpty(); return element; }

    /**
     * <summary> Clears this object to its blank/initial state.</summary>
     *
     * <remarks> Tobi, 3/16/2012.</remarks>
     */
    public void Clear()
    {
      _data.Clear();
      IsEmpty = true;
    }
    /**
     * <summary> Gets the stack elements.</summary>
     *
     * <value> The stack elements.</value>
     */
    public IEnumerable<T> StackElements { get { return _data; } }

    /**
     * <summary> Default constructor.</summary>
     *
     * <remarks> Tobi, 3/16/2012.</remarks>
     */
    public ObservableStack()
    {
      _data = CollectionFactory.CreateCollection<T>(true);
    }

    /**
     * <summary> Gets or sets a value indicating whether this stack is empty.</summary>
     *
     * <value> true if this object is empty, false if not.</value>
     */
    public bool IsEmpty
    {
      get
      {
        return _isEmpty;
      }
      private set
      {
        if (_isEmpty == value) return;
        _isEmpty = value;
        RaisePropertyChanged();
        if (!IsEmpty && StackFilled != null) StackFilled(this);
        if (IsEmpty && StackEmptied != null) StackEmptied(this);
      }
    }
    /// <summary> Event queue for all listeners interested in StackEmptied events. Which is raised when IsEmpty changes from false to true</summary>
    public event Action<ObservableStack<T>> StackEmptied;
    /// <summary> Event queue for all listeners interested in StackFilled events. Which is raised when the IsEmpty changed from true to false </summary>
    public event Action<ObservableStack<T>> StackFilled;

    #region private 
    
    private void CheckEmpty()
    {
      if (_data.Count == 0) IsEmpty = true;
      else IsEmpty = false;
    }
    private ICollection<T> _data;
    private bool _isEmpty = true;
#endregion

    
  }

}
