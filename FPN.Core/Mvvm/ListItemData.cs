namespace FPN.Core.Mvvm
{
	public class ListItemData
	{
		public ListItemData(object value, string displayValue)
		{
			Value = value;
			DisplayValue = displayValue;
		}

		public ListItemData(object value, object displayValue)
		{
			Value = value;
			DisplayValue = displayValue.ToString() ?? string.Empty;
		}

		public object Value { get; set; }
		public string DisplayValue { get; set; }
	}
}