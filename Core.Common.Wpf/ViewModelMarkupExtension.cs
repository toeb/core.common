using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Core.Common.Wpf
{

  

  [MarkupExtensionReturnType(typeof(object))]
  public class ViewModelMarkupExtension : MarkupExtension
  {
  
    public ViewModelMarkupExtension(object Model)
    {
      this.Model = Model as Binding;
    }
  
    public Type ViewModelType { get; set; }
  
    
    public string Contract { get; set; }
    [ConstructorArgument("Model")]
    public Binding Model { get; set; }
  
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      if (serviceProvider == null) return null;
      var valueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
  
      if (!(valueTarget.TargetObject is FrameworkElement)) return this;
  
      Model.Converter = new ViewModelConverter(
        valueTarget.TargetObject,
        valueTarget.TargetProperty,
        ViewModelType,
        Contract
        );
  
      var bindingValue = Model.ProvideValue(serviceProvider);
  
  
      return bindingValue;
  
  
  
  
  
  
    }
  }
}
