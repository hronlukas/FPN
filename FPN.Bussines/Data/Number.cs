using System.Collections.Generic;

namespace FPN.Bussines.Data
{
	internal class Number : INumber
	{
		public string No { get; set; }
		public ITariff Tariff { get; set; }
		public IReadOnlyList<IAction> Actions { get; set; }
	}
}