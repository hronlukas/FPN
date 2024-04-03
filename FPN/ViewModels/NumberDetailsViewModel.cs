using FPN.Bussines.Data;
using FPN.Bussines.Services;
using FPN.Converters;
using FPN.Core.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FPN.ViewModels
{
	public class NumberDetailsViewModel(ICalculatorProvider calculatorProvider) : ViewModelBase
	{
		private readonly ICalculatorProvider calculatorProvider = calculatorProvider;

		public IEnumerable? NumberDetails { get; set; }

		public INumber? SelectedNumber { get; set; }

		public void SetData(IInvoice invoice, INumber selectedNumber)
		{
			SelectedNumber = selectedNumber;

			var actionsCalculator = calculatorProvider.GetCalculator(invoice);

			NumberDetails = BuildNumberDetails(actionsCalculator, selectedNumber);
		}

		private static IList<ListItemData> BuildNumberDetails(IActionsCalculator actionsCalculator, INumber number)
		{
			var nationalCalls = GetCallsAmount(actionsCalculator.GetNationalCalls(number));
			var internationalCalls = GetCallsAmount(actionsCalculator.GetInterNationalCalls(number));
			var fpnCalls = GetCallsAmount(actionsCalculator.GetFpnCalls(number));
			return
			[
				new ListItemData("Národní sítě", nationalCalls),
				new ListItemData("Mezinárodní sítě", internationalCalls),
				new ListItemData("FPN", fpnCalls),
			];
		}

		private static TimeSpan GetCallsAmount(IEnumerable<ICall> calls)
		{
			var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
			var callsAmount = calls.Sum(c => c.Amount);
			var callsTimeSpan = (TimeSpan)SecondsToTimeSpanConverter.Instance.Convert(callsAmount, typeof(TimeSpan), null, culture);
			return callsTimeSpan;
		}
	}
}