using FPN.Bussines.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FPN.Converters
{
	internal class ActionTypeConverter : IValueConverter
	{
		private static IValueConverter? instance;

		public static IValueConverter? Instance => instance ??= new ActionTypeConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
		{
			ICall => "Call",
			ISms => "SMS",
			_ => Binding.DoNothing,
		};

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}