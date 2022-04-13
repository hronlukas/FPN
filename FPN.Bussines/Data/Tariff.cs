namespace FPN.Bussines.Data
{
	internal class Tariff : ITariff
	{
		private const string SharedTariffName = "Sdílený tarif pro podnikání - clen";
		private const string CustomTariffName = "Tarif na míru";

		public string Name { get; set; }

		public TariffPlan Plan
		{
			get
			{
				if (Name == SharedTariffName)
				{
					return TariffPlan.Shared;
				}
				else if (Name == CustomTariffName)
				{
					return TariffPlan.Custom;
				}

				//throw new NotImplementedException("Unknown tariff plan");
				return TariffPlan.Unknown;
			}
		}
	}
}