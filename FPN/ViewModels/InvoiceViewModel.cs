using FPN.Bussines.Data;
using FPN.Bussines.Services;
using FPN.Converters;
using FPN.Core.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace FPN.ViewModels
{
	public class InvoiceViewModel(ICalculatorProvider calculatorProvider) : ViewModelBase
	{
		private readonly ICalculatorProvider calculatorProvider = calculatorProvider;
		private string? filter;

		public IEnumerable? Details { get; set; }

		public string? Filter
		{
			get => filter;
			set
			{
				filter = value;
				Numbers?.Refresh();
			}
		}

		public IInvoice? Invoice { get; set; }

		public ICollectionView? Numbers { get; set; }

		public INumber? SelectedNumber { get; set; }

		public void SetInvoice(IInvoice invoice)
		{
			Invoice = invoice;
			Numbers = CollectionViewSource.GetDefaultView(invoice.Numbers);
			if (invoice.Numbers.Count > 0)
			{
				SelectedNumber = invoice.Numbers[0];
			}

			Numbers.Filter = i =>
			{
				if (!string.IsNullOrEmpty(Filter) && i is INumber n)
				{
					return n.No.Contains(Filter);
				}

				return true;
			};

			var actionsCalculator = calculatorProvider.GetCalculator(invoice);

			Details = BuildDetails(invoice, actionsCalculator);
		}

		private static IEnumerable BuildDetails(IInvoice invoice, IActionsCalculator actionsCalculator)
		{
			var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
			var freeSeconds = actionsCalculator.GetFreeSecondsAmount();
			var freeTimeSpan = (TimeSpan)SecondsToTimeSpanConverter.Instance.Convert(freeSeconds, typeof(TimeSpan), null, culture);
			var freeSms = actionsCalculator.GetFreeSmsAmount();
			var sharedNumbers = actionsCalculator.GetNumbersWithSharedTariff();
			var sharedNumbersCount = sharedNumbers.Count();
			var freeMinutesPerNumber = freeTimeSpan.TotalMinutes / sharedNumbersCount;
			var totalCalledUnits = sharedNumbers.Sum(n => actionsCalculator.GetNationalCalls(n).Sum(a => a.Amount));
			var totalCalledTimeSpan = (TimeSpan)SecondsToTimeSpanConverter.Instance.Convert(totalCalledUnits, typeof(TimeSpan), null, culture);
			var totalNationSmsUnits = sharedNumbers.Sum(n => actionsCalculator.GetNationalSms(n).Sum(a => a.Amount));
			var totalFpnSmsUnits = sharedNumbers.Sum(n => actionsCalculator.GetFpnSms(n).Sum(a => a.Amount));
			return new List<ListItemData>()
			{
				new("Od", invoice.From.ToShortDateString()),
				new("Do", invoice.To.ToShortDateString()),
				new("Volné minuty", freeTimeSpan.TotalMinutes),
				new("Volné SMS", freeSms),
				new("Počet čísel se sdíleným tarifem", sharedNumbersCount),
				new("Počet volných minut / číslo", Math.Round(freeMinutesPerNumber)),
				new("Počet provolaných minut", totalCalledTimeSpan.TotalMinutes),
				new("Počet poslaných SMS do ostatních sítí", totalNationSmsUnits),
				new("Počet poslaných SMS do FPN", totalFpnSmsUnits),
			}.AsReadOnly();
		}
	}
}