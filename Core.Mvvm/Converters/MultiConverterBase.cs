using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace Core.Common.Wpf.Converters
{
	public abstract class MultiConverterBase : MarkupExtension, IMultiValueConverter
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		public abstract object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture);

		public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			return targetTypes.Select(t => Binding.DoNothing).ToArray();

		}
	}
}
