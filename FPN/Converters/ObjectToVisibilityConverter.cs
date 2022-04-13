using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FPN.Converters
{
	internal class ObjectToVisibilityConverter : IValueConverter
	{
		private static IValueConverter? instance;

		public static IValueConverter Instance => instance ??= new ObjectToVisibilityConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var visible = value != null;
			if (parameter is string s && s == "!")
			{
				visible = !visible;
			}

			return visible
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}