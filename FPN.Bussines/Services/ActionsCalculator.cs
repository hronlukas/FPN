using FPN.Bussines.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FPN.Bussines.Services
{
	internal sealed class ActionsCalculator : IActionsCalculator
	{
		private const string CountryCodeCR = "420";
		private readonly IInvoice invoice;

		public ActionsCalculator(ILogger<ActionsCalculator> logger, IInvoice invoice)
		{
			logger.LogDebug("Ctor");
			this.invoice = invoice;
		}

		public IEnumerable<ICall> GetFpnCalls(INumber number)
		{
			return GetFpnActions<ICall>(number);
		}

		public IEnumerable<ISms> GetFpnSms(INumber number)
		{
			return GetFpnActions<ISms>(number);
		}

		public int GetFreeSecondsAmount()
		{
			// TODO read from invoice
			return 2000 * 60;
		}

		public int GetFreeSmsAmount()
		{
			// TODO read from invoice
			return 500;
		}

		public IEnumerable<ICall> GetInterNationalCalls(INumber number)
		{
			return number.Actions
				.OfType<ICall>()
				.Where(call => !call.TargetNo.StartsWith(CountryCodeCR));
		}

		public IEnumerable<ICall> GetNationalCalls(INumber number)
		{
			return GetNationalActions<ICall>(number);
		}

		public IEnumerable<ISms> GetNationalSms(INumber number)
		{
			return GetNationalActions<ISms>(number);
		}

		public IEnumerable<INumber> GetNumbersWithSharedTariff()
		{
			return invoice.Numbers.Where(n => n.Tariff.Plan == TariffPlan.Shared);
		}

		private IEnumerable<ActionType> GetFpnActions<ActionType>(INumber number) where ActionType : IAction
		{
			return number.Actions
				.OfType<ActionType>()
				.Where(call => invoice.Numbers.Any(n => n.No == call.TargetNo));
		}

		private IEnumerable<ActionType> GetNationalActions<ActionType>(INumber number) where ActionType : IAction
		{
			return number.Actions
				.OfType<ActionType>()
				.Where(call => call.TargetNo.StartsWith(CountryCodeCR))
				.Except(GetFpnActions<ActionType>(number));
		}
	}
}