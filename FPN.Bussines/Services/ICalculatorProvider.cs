using FPN.Bussines.Data;

namespace FPN.Bussines.Services
{
	public interface ICalculatorProvider
	{
		IActionsCalculator GetCalculator(IInvoice invoice);
	}
}