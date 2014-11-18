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

    protected override void SetProperty(string key, object value, Type type)
    {
      var prop = HasProperty(key,type) ? GetProperty(key,type) : null;
      base.SetProperty(key, value,type);
      if (prop != value)
      {
        OnPropertyChanaged(key);
        RaisePropertyChanged(key);
      }
    }
    protected void RaisePropertyChanged(string key)
    {

      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(key));
    }

    protected virtual void OnPropertyChanaged(string key){}

    public event PropertyChangedEventHandler PropertyChanged;
  }
}