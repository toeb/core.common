using System.ComponentModel;
using Core.Values;
using System;

namespace Core
{
  /// <summary>
  /// implementation of scope that raises a property changed event when a property is changed.
  /// </summary>
  public class PropertyChangingScope : Core.PropertyStoreBase, INotifyPropertyChanged
  {
    public PropertyChangingScope()
    {
    }
    public PropertyChangingScope(IValueMap implementation) : base(implementation) { }

    protected override bool TrySetProperty(string key, object value, Type type)
    {
      object oldValue;
      var getSuccess = TryGetProperty(key, out oldValue, type);

      var setSuccess = base.TrySetProperty(key, value, type);
      if (!setSuccess) return false;
      if (getSuccess && oldValue != value)
      {
        OnPropertyChanaged(key);
        RaisePropertyChanged(key);
      }

      return true;
    }
    protected void RaisePropertyChanged(string key)
    {

      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(key));
    }

    protected virtual void OnPropertyChanaged(string key){}

    public event PropertyChangedEventHandler PropertyChanged;
  }
}