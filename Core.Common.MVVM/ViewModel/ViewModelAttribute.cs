using System;
using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
  public class ViewModelAttribute : ExportAttribute, IViewModelMetadata
  {
    public ViewModelAttribute(Type type):base(typeof(IViewModel))
    {
      ViewModelType = type;        
      Contract = type.FullName;
    }
  
    public Type ModelType
    {
      get;
      set;
    }
  
    public string Contract
    {
      get;
      set;
    }
  
    public Type ViewModelType
    {
      get;
      set;
    }
  
  
  }
}
