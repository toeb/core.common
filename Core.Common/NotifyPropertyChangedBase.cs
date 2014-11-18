using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using System.Runtime.CompilerServices;
namespace Core
{

  public delegate bool AddElement<T>(T item);
  public delegate bool RemoveElement<T>(T item);


  public class NotifyPropertyChangedBase : SubscribableBase, INotifyPropertyChanged, ISubscribable
  {
    protected sealed override void OnNotifyTopicChanged(string property, object oldValue, object newValue)
    {
      base.OnNotifyTopicChanged(property, oldValue, newValue);

    }
    protected sealed override void OnNotifyTopicChanging(string property, object oldValue, object newValue)
    {

    }
    protected void RaisePropertyChanged(Lazy<PropertyChangedEventArgs> args)
    {
      if (PropertyChanged == null) return;
      RaisePropertyChanged(args.Value);
    }
    protected void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
      if (PropertyChanged == null) return;
      PropertyChanged(this, args);
    }
    protected void RaisePropertyChanged(string propertyName)
    {
      if (PropertyChanged == null) return;
      RaisePropertyChanged(new Lazy<PropertyChangedEventArgs>(() => new PropertyChangedEventArgs(propertyName)));
    }
    protected void RaisePropertyChanged()
    {
      if (PropertyChanged == null) return;
      var name = Reflection.GetCallingPropertyName();
      RaisePropertyChanged(name);
    }

    protected bool ChangeIfDifferent<T>(ref T originalValue, T newValue, [CallerMemberName]string propertyName = null)
    {
      if (propertyName == null) propertyName = Reflection.GetCallingPropertyName();
      if (Equality.Check(originalValue, newValue)) return false;
      var tmp = originalValue;
      NotifyTopicChanging(propertyName, tmp, newValue);
      originalValue = newValue;
      NotifyTopicChanged(propertyName, tmp, newValue);
      RaisePropertyChanged(propertyName);
      return true;
    }
    protected bool ChangeIfDifferentAndCallback<T>(ref T valueReference, T newValue, OnValueChangeDelegate<T> changing, OnValueChangeDelegate<T> changed, string propertyName)
    {

      if (Equality.Check(valueReference, newValue)) return false;
      var oldValue = valueReference;
      if (changing != null) changing(oldValue, newValue);
      NotifyTopicChanging(propertyName, oldValue, newValue);
      valueReference = newValue;
      if (changed != null) changed(oldValue, newValue);
      NotifyTopicChanged(propertyName, oldValue, newValue);
      RaisePropertyChanged(propertyName);
      return true;
    }
    protected bool ChangeIfDifferent<T>(ProduceValueDelegate<T> currentValueDelegate, ConsumeValueDelegate<T> consumeDelegate, T newValue, OnValueChangeDelegate<T> changing, OnValueChangeDelegate<T> changed, string propertyName)
    {
      var oldValue = currentValueDelegate();
      if (Equality.Check(oldValue, newValue)) return false;
      if (changing != null) changing(oldValue, newValue);
      NotifyTopicChanging(propertyName, oldValue, newValue);
      consumeDelegate(newValue);
      if (changed != null) changed(oldValue, newValue);
      NotifyTopicChanged(propertyName, oldValue, newValue);
      RaisePropertyChanged(propertyName);
      return true;
    }
    protected bool ChangeIfDifferentAndCallback<T>(ref T originalValue, T newValue, OnValueChangeDelegate<T> changing, OnValueChangeDelegate<T> changed)
    {
      return ChangeIfDifferentAndCallback(ref originalValue, newValue, changing, changed, Reflection.GetCallingPropertyName());
    }
    protected bool TransformAndChange<T>(ref T originalValue, T newValue, TransformValueDelegate<T, T> transform, OnValueChangeDelegate<T> changing, OnValueChangeDelegate<T> changed)
    {
      var propertyName = Reflection.GetCallingPropertyName();
      return TransformAndChange(ref originalValue, newValue, transform, changing, changed, propertyName);
    }
    protected bool TransformAndChange<T>(ref T originalValue, T newValue, TransformValueDelegate<T, T> transform, OnValueChangeDelegate<T> changing, OnValueChangeDelegate<T> changed, string propertyName)
    {
      var transformed = newValue;
      if (transform != null) transformed = transform(newValue);
      return ChangeIfDifferentAndCallback(ref originalValue, transformed, changing, changed, propertyName);
    }



    protected bool ChangeEnumerable<T>(
      ref IEnumerable<T> storedItems,
      IEnumerable<T> incommingItems,
      ElementsAdded<T> addedCallback,
      ElementsRemoved<T> removedCallback,
      string propertyName = null
      )
    {
      if (storedItems == incommingItems) return false;
      if (propertyName == null) propertyName = Reflection.GetCallingPropertyName();
      var originalItems = storedItems ?? new T[0];
      var newItems = incommingItems ?? new T[0];

      ChangeIfDifferent(ref storedItems, incommingItems, propertyName);
      if (removedCallback != null)
      {
        var removedvalues = originalItems.Except(newItems);
        removedCallback(removedvalues);
      }
      if (addedCallback != null)
      {
        var addedItems = newItems.Except(originalItems);
        addedCallback(addedItems);
      }
      return true;
    }

    protected bool ChangeCollectionByEnumerableValue<T>(ICollection<T> items, IEnumerable<T> newItems, [CallerMemberName] string propertyName = null)
    {
      return ChangeEnumerableCallback(items, newItems, it =>
      {
        items.Add(it);
        return true;
      }, it =>
      {
        items.Remove(it);
        return true;
      }, propertyName);
    }
    protected bool ChangeSetByEnumerableValue<T>(ISet<T> items, IEnumerable<T> newItems, [CallerMemberName] string propertyName = null)
    {
      return ChangeEnumerableCallback(items, newItems, it =>
      {
        return items.Add(it);

      }, it =>
      {
        return items.Remove(it);
      }, propertyName);
    }

    protected bool ChangeEnumerableCallback<T>(IEnumerable<T> oldItems, IEnumerable<T> newItems, AddElement<T> addCallback, RemoveElement<T> removeCallback, string property = null)
    {
      var toAdd = newItems.Except(oldItems);
      var toRemove = oldItems.Except(newItems);

      bool changed = false;

      foreach (var item in toAdd)
      {
        changed |= addCallback(item);
      }
      foreach (var item in toRemove)
      {
        changed |= removeCallback(item);
      }
      property = property ?? Reflection.GetCallingPropertyName();
      if (changed) RaisePropertyChanged(property);
      return changed;
    }

    protected bool ChangeEnumerableSingleItemCallback<T>(
      ref IEnumerable<T> storedItems,
      IEnumerable<T> newItems,
      ElementAdded<T> addedCallback,
      ElementRemoved<T> removedCallback,
      string propertyName = null
      )
    {
      return ChangeEnumerable(
        ref storedItems,
        newItems,
        items => items.Do(i => addedCallback(i)),
        items => items.Do(i => removedCallback(i)),
        propertyName);
    }


    public event PropertyChangedEventHandler PropertyChanged;

  }
}
