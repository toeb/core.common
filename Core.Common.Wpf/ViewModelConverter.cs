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
  public class ViewModelConverter : IValueConverter
  {
    private object targetObject;
    private object targetProperty;
    private string viewModelContract;
    private Type viewModelType;
  
  
    public ViewModelConverter(object targetObject, object targetProperty, Type viewModelType, string viewModelContract)
    {
      // TODO: Complete member initialization
      this.targetObject = targetObject;
      this.targetProperty = targetProperty;
      this.viewModelType = viewModelType;
      this.viewModelContract = viewModelContract;
    }
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == null) return null;
  
      var view = targetObject as DependencyObject;
      if (view == null) return null;
      
      IViewModel parentViewModel;
      while(true){
        if(view==null)return null;
        var fw = view as FrameworkElement;
        if(fw!=null){
          if(fw.DataContext is IViewModel){
            parentViewModel = fw.DataContext as IViewModel;
            break;
          }
        }
        view = VisualTreeHelper.GetParent(view);
  
        
      }
  
  
  
  
      if (parentViewModel == null) throw new InvalidOperationException("cannot create viewmodel for " + value + " because parent viewmodel could not be determined");
  
      var viewmodel = parentViewModel.RequireChild(value, viewModelType, viewModelContract);
      if (viewmodel == null) throw new InvalidOperationException("could not create viewmodel for " + value);
  
      return viewmodel;
    }
  
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var viewModel = value as IViewModel;
      if (viewModel == null) throw new InvalidOperationException("could not convert back " + value);
      return viewModel.Model;
    }
  
  }
}
