using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Core.Common.MVVM
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
  public class ViewAttribute : ExportAttribute, IViewMetadata
  {
    public ViewAttribute(Type viewType)
      : base(typeof(IView))
    {
      this.ViewType = viewType;
      this.Contract = ViewType.FullName;  
    }
    public ViewAttribute(Type modelType, Type viewType, Type viewModelType):this(viewType,viewModelType)
    {
      ModelType = modelType;
  
    }
    public ViewAttribute(Type viewType, Type viewModelType):this(viewType)
    {
      ViewModelType = viewModelType;
      
    }
    public Type ViewModelType { get; set; }
    public Type ViewType { get; set; }
    public Type ModelType { get; set; }
    public string Contract { get; set; }
  }
}
