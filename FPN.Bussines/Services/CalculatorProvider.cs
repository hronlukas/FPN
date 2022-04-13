using FPN.Bussines.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FPN.Bussines.Services
{
	internal sealed class CalculatorProvider : ICalculatorProvider
	{
		private readonly ILogger<CalculatorProvider> logger;
		private readonly ILogger<ActionsCalculator> actionsCalculatorLogger;

		private Dictionary<IInvoice, IActionsCalculator> calculators = new Dictionary<IInvoice, IActionsCalculator>();

		public CalculatorProvider(ILogger<CalculatorProvider> logger, ILogger<ActionsCalculator> actionsCalculatorLogger)
		{
			logger.LogDebug("Ctor");
			this.logger = logger;
			this.actionsCalculatorLogger = actionsCalculatorLogger;
		}

		public IActionsCalculator GetCalculator(IInvoice invoice)
		{
			if (!calculators.TryGetValue(invoice, out IActionsCalculator actionsCalculator))
			{
				logger.LogDebug("Creating new actions calculator for invoice {from}", invoice.From);
				actionsCalculator = new ActionsCalculator(actionsCalculatorLogger, invoice);
				calculators.Add(invoice, actionsCalculator);
			}

			return actionsCalculator;
		}
	}
}