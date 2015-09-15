using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Common
{
  public class ValidatingSetter : DelegatingPropertySetter, IDataErrorInfo, INotifyDataErrorInfo
  {
    public ValidatingSetter(PropertySetterDelegate inner) : base(inner) { }
    protected override bool Set(object @object, string key, Type type, object value)
    {
      ICollection<ValidationResult> validationResults = new List<ValidationResult>();
      var success = Inner(@object, key, type, value);
      Validator.TryValidateProperty(value, new ValidationContext(@object, null,null) { MemberName = key }, validationResults);
      RaiseErrorsChanged(@object, key, validationResults);
      return success;
    }
  
  
    private void RaiseErrorsChanged(object sender, string key, IEnumerable<ValidationResult> errors)
    {
  
      var toRemove = this.errors.Where(error => error.MemberNames.Contains(key)).ToArray();
      if (toRemove.Count() == 0 && errors.Count() == 0) return;
      foreach (var item in toRemove) this.errors.Remove(item);
      foreach (var item in errors) this.errors.Add(item);
      if (ErrorsChanged != null) ErrorsChanged(this, new DataErrorsChangedEventArgs(key));
    }
  
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    private List<ValidationResult> errors = new List<ValidationResult>();
    public System.Collections.IEnumerable GetErrors(string propertyName)
    {
      return errors.Where(vr => vr.MemberNames.Any(mn => mn == propertyName)).Select(vr => vr.ErrorMessage).ToArray();
    }
  
    public bool HasErrors
    {
      get { return errors.Any(); }
    }
  
  
    public string GetErrorMessage(string columnName)
    {
      var errors = GetErrors(columnName).Cast<string>();
      return string.Join(", ", errors);
    }
  
    public string GetErrorMessage()
    {
      return string.Join("\n", errors.Select(vr => "* " + string.Join(", ", vr.MemberNames) + ": " + vr.ErrorMessage));
    }
  
    public string Error
    {
      get { return GetErrorMessage(); }
    }
  
    public string this[string columnName]
    {
      get { return GetErrorMessage(columnName); }
    }
  }
}
