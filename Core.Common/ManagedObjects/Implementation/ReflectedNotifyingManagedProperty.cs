using System;
using System.ComponentModel;
using System.Reflection;

namespace Core.ManagedObjects
{
  public class ReflectedNotifyingManagedProperty : ReflectedManagedProperty
  {
    public ReflectedNotifyingManagedProperty(object instance, ReflectedManagedPropertyInfo info)
      : base(instance, info)
    {
      if (!(instance is INotifyPropertyChanged)) throw new ArgumentException("instance is not of type INotifyPropertyChanged");
      Notifier = instance as INotifyPropertyChanged;
      Notifier.PropertyChanged += PropertyChangedCallback;
    }
    private void PropertyChangedCallback(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName != PropertyInfo.Name) return;
      NotifyValueChanged();
    }
    ~ReflectedNotifyingManagedProperty()
    {
      Notifier.PropertyChanged -= PropertyChangedCallback;
    }

    private INotifyPropertyChanged Notifier { get; set; }
  }
}
