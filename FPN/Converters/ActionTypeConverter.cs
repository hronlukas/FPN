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

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case ICall:
					return "Call";

				case ISms:
					return "SMS";
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}