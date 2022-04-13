using System;
using System.Globalization;
using System.Windows.Data;

namespace FPN.Converters
{
	internal class SecondsToTimeSpanConverter : IValueConverter
	{
		private static IValueConverter? instance;

		public static IValueConverter Instance => instance ??= new SecondsToTimeSpanConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var sec = (int)value;
			var ts = TimeSpan.FromSeconds(sec);
			return ts;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}