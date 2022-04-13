using System;
using System.Collections.Generic;

namespace FPN.Bussines.Data
{
	public interface IInvoice
	{
		DateTime From { get; }
		DateTime To { get; }
		IReadOnlyList<INumber> Numbers { get; }
	}
}