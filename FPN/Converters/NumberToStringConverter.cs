using System;
using System.Globalization;
using System.Windows.Data;

namespace FPN.Converters
{
	internal class NumberToStringConverter : IValueConverter
	{
		private static IValueConverter? instance;

		public static IValueConverter Instance => instance ??= new NumberToStringConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var number = value as string;
			return number?.Substring(0, 3) + "-" + number?.Substring(3);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}