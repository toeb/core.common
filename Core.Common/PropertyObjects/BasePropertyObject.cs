using System;
using System.ComponentModel;

namespace Core.Common
{



  public class BasePropertyObject : ObjectBase, INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
  {
    private ValidatingSetter validator;
  
    public BasePropertyObject()
    {
  
      getter = new DefaultValueGetter(getter);
      setter = new NotifyPropertyChangedSetter(RaisePropertyChanged, getter, setter);
      setter = new CancelPropertyChangeSetter(RaisePropertyCancel, getter, setter);
      setter = validator = new ValidatingSetter(setter);
    }

    protected virtual bool RaisePropertyCancel(string key, object oldValue, object newValue)
    {
      return false;
    }
  
  
  
    protected void RaisePropertyChanged(string key)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(key));
    }
  
    public event PropertyChangedEventHandler PropertyChanged;
  
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
    {
      add { validator.ErrorsChanged += value; }
      remove { validator.ErrorsChanged -= value; }
    }
  
    public System.Collections.IEnumerable GetErrors(string propertyName)
    {
      return validator.GetErrors(propertyName);
    }
  
    public bool HasErrors
    {
      get { return validator.HasErrors; }
    }
  
  
    string IDataErrorInfo.Error
    {
      get { return validator.GetErrorMessage(); }
    }
  
    string IDataErrorInfo.this[string columnName]
    {
      get { return validator.GetErrorMessage(columnName); ; }
    }
  }
}
