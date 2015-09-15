using System;

namespace Core.Common
{
  public class DataErrorsChangedEventArgs :EventArgs
  {
    public DataErrorsChangedEventArgs(string propertyName) { this.PropertyName = propertyName; }
    public string PropertyName { get; set; }
  }
}
