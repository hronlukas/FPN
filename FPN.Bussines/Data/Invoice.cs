using System;
using System.Collections.Generic;

namespace FPN.Bussines.Data
{
	internal class Invoice : IInvoice
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public IReadOnlyList<INumber> Numbers { get; set; }
	}
}