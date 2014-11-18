using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Core.Collections
{
  public class ObservableCollectionView<T> : CollectionView<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    public ObservableCollectionView(ICollection<T> collection, Func<T, bool> predicate)
      : base(collection, predicate)
    {
      var notifyingCollection = collection as INotifyCollectionChanged;
      var notifyingObject = collection as INotifyPropertyChanged;
      if (notifyingCollection != null) notifyingCollection.CollectionChanged += changeHandler;
      if (notifyingObject != null) notifyingObject.PropertyChanged += propertyChanged;
    }

    private void propertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "Count":
        case "IsReadOnly":
          break;
        default: return;
      }
      if (PropertyChanged != null)
      {
        PropertyChanged(this, e);
      }
    }

    ~ObservableCollectionView()
    {
      var notifyingCollection = InnerCollection as INotifyCollectionChanged;
      var notifyingObject = InnerCollection as INotifyPropertyChanged;

      if (notifyingCollection != null) notifyingCollection.CollectionChanged -= changeHandler;
      if (notifyingObject != null) notifyingObject.PropertyChanged -= propertyChanged;
    }
    private void changeHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyCollectionChangedEventArgs args = null;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          {
            var items = e.NewItems.OfType<T>().Where(Predicate).ToList();
            if (!items.Any()) return;
            args = new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Add,
              items
              );
            break;
          }
        case NotifyCollectionChangedAction.Remove:
          {
            var items = e.OldItems.OfType<T>().Where(Predicate).ToList();
            if (!items.Any()) return;
            args = new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Remove,
              items
              );
            break;
          }
        case NotifyCollectionChangedAction.Replace:
          {
            var newItems = e.NewItems.OfType<T>().Where(Predicate).ToList();
            var oldItems = e.OldItems.OfType<T>().Where(Predicate).ToList();

            if (newItems.None() && oldItems.None()) return;
            if (!newItems.Any())
            {
              args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems);
              break;
            }
            if (!oldItems.Any())
            {
              args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems);
              break;
            }
            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems);
            break;
          }
        case NotifyCollectionChangedAction.Reset:
          args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
          break;
      }
      if (args == null) return;
      if (CollectionChanged != null) CollectionChanged(this, args);
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public event PropertyChangedEventHandler PropertyChanged;
  }

}
