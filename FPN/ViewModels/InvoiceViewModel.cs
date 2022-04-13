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
	internal class InvoiceViewModel : ViewModelBase
	{
		private readonly ICalculatorProvider calculatorProvider;
		private string? filter;

		public InvoiceViewModel(ICalculatorProvider calculatorProvider)
		{
			this.calculatorProvider = calculatorProvider;
		}

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
			SelectedNumber = invoice.Numbers.FirstOrDefault();

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

		private IEnumerable BuildDetails(IInvoice invoice, IActionsCalculator actionsCalculator)
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
				new ListItemData("Od", invoice.From.ToShortDateString()),
				new ListItemData("Do", invoice.To.ToShortDateString()),
				new ListItemData("Volné minuty", freeTimeSpan.TotalMinutes),
				new ListItemData("Volné SMS", freeSms),
				new ListItemData("Počet čísel se sdíleným tarifem", sharedNumbersCount),
				new ListItemData("Počet volných minut / číslo", Math.Round(freeMinutesPerNumber)),
				new ListItemData("Počet provolaných minut", totalCalledTimeSpan.TotalMinutes),
				new ListItemData("Počet poslaných SMS do ostatních sítí", totalNationSmsUnits),
				new ListItemData("Počet poslaných SMS do FPN", totalFpnSmsUnits),
			}.AsReadOnly();
		}
	}
}