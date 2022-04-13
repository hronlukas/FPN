using FPN.Bussines.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FPN.Bussines.Services
{
	public class XmlInvoiceConverter : IDataConverter
	{
		private const string Amount = "units";
		private const string InvoiceElementName = "INVOICE";

		#region Summary

		private const string InvoiceSummaryElementName = "INV_SUM";
		private const string InvoiceSummaryFromElementName = "I_FROM";
		private const string InvoiceSummaryToElementName = "I_TO";

		#endregion Summary

		#region Numbers

		private const string NumberAttributeName = "NUMBER";
		private const string NumberElementName = "MSISDN_MAIN";
		private const string NumbersElementName = "CUR_INV";
		private const string TariffAttributeName = "TARIFF_PLAN";

		#endregion Numbers

		#region Actions

		private const string ActionElementName = "CDR_U";
		private const string ActionsElementName = "CDR_SUM";
		private const string ActionType1AttrName = "LEV1_ID";
		private const string TargetNumber = "numberCalled";

		#endregion Actions

		private const string Unknown = "-";
		private readonly ILogger logger;

		public XmlInvoiceConverter(ILogger<XmlInvoiceConverter> logger)
		{
			this.logger = logger;
		}

		private enum ActionType
		{
			Call = 1000,
			Sms = 5000,
			Mms = 6000,
		}

		public IInvoice Transform(XDocument input)
		{
			if (input is null)
			{
				throw new ArgumentNullException(nameof(input));
			}

			logger.LogInformation("Start data transformation");

			var invoice = new Invoice();

			var invoiceElement = input.Element(InvoiceElementName);
			var summary = invoiceElement.Element(InvoiceSummaryElementName);

			invoice.From = ParseDate(summary.Element(InvoiceSummaryFromElementName).Value);
			invoice.To = ParseDate(summary.Element(InvoiceSummaryToElementName).Value);

			invoice.Numbers = ReadNumbers(invoiceElement.Element(NumbersElementName));

			return invoice;
		}

		private IAction CreateAction(ActionType type, XElement a)
		{
			switch (type)
			{
				case ActionType.Call:
					return CreateCall(a);

				case ActionType.Sms:
					return CreateSms(a);

				case ActionType.Mms:
					return CreateMms(a);
			}

			//throw new ArgumentOutOfRangeException(nameof(type));
			return null;
		}

		private IAction CreateCall(XElement a)
		{
			return new Call
			{
				TargetNo = a.Attribute(TargetNumber).Value,
				Amount = GetCallAmount(a),
				StartDate = ParseDateAndTime(a.Attribute("startDateEvent").Value, a.Attribute("startTimeEvent").Value)
			};
		}

		private IAction CreateMms(XElement a)
		{
			return new Mms
			{
				TargetNo = a.Attribute(TargetNumber).Value,
				StartDate = ParseDateAndTime(a.Attribute("startDateEvent").Value, a.Attribute("startTimeEvent").Value)
			};
		}

		private IAction CreateSms(XElement a)
		{
			return new Sms
			{
				TargetNo = a.Attribute(TargetNumber).Value,
				StartDate = ParseDateAndTime(a.Attribute("startDateEvent").Value, a.Attribute("startTimeEvent").Value),
				Amount = GetCallAmount(a),
			};
		}

		private ActionType GetActionType(string value)
		{
			var actionType = (ActionType)Enum.Parse(typeof(ActionType), value);
			return actionType;
		}

		private int GetCallAmount(XElement a)
		{
			// todo precitst unitsType, (nyni tam je SEC) a pripadne zkonvertovat na sec...
			return int.Parse(a.Attribute(Amount).Value);
		}

		private DateTime ParseDate(string value)
		{
			if (DateTime.TryParse(value, out var date))
			{
				return date;
			}

			logger.LogWarning("Unable to parse  date {value}", value);
			return DateTime.MinValue;
		}

		private DateTime ParseDateAndTime(string date, string time)
		{
			if (DateTime.TryParse($"{date} {time}", out var dt))
			{
				return dt;
			}

			logger.LogWarning("Unable to parse date and time {date} {time}", date, time);
			return DateTime.MinValue;
		}

		private IReadOnlyList<IAction> ReadActions(XElement n)
		{
			var actions = n.Element(ActionsElementName);
			return actions
				.Elements(ActionElementName)
				.Select(a =>
				{
					var type = GetActionType(a.Attribute(ActionType1AttrName).Value);
					return CreateAction(type, a);
				}).Where(a => a != null).ToList().AsReadOnly();
		}

		private IReadOnlyList<INumber> ReadNumbers(XElement numbers)
		{
			return numbers
				.Elements(NumberElementName)
				.Select(n =>
				{
					return new Number
					{
						No = n.Attribute(NumberAttributeName).Value,
						Tariff = new Tariff { Name = n.Attribute(TariffAttributeName)?.Value ?? Unknown },
						Actions = ReadActions(n),
					};
				})
				.ToList().AsReadOnly();
		}
	}
}