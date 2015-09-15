using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Core.Common.Wpf.Converters
{
  public class NullToVisibilityConverter:IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (targetType != typeof(Visibility)) throw new InvalidOperationException("target type is not visibility");

      var notVisible = Visibility.Hidden;
      if (parameter != null)
      {
        if (!(parameter is Visibility))
        {
          throw new InvalidOperationException("cannot convert parameter to a visibility");
        }
        notVisible = (Visibility)parameter;
      }

      if (value == null) return notVisible;

      return Visibility.Visible;

        
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
