using System;

namespace Core.Common
{
  public interface INotifyDataErrorInfo
  {
    event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
  
    System.Collections.IEnumerable GetErrors(string propertyName);
  
    bool HasErrors { get; }
  }
}
