using System.Collections.Generic;

namespace FPN.Bussines.Data
{
	public interface INumber
	{
		string No { get; }
		ITariff Tariff { get; }
		IReadOnlyList<IAction> Actions { get; }
	}
}