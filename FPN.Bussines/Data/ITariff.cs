namespace FPN.Bussines.Data
{
	public interface ITariff
	{
		string Name { get; }

		TariffPlan Plan { get; }
	}
}