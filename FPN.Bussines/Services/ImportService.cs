using FPN.Bussines.Data;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Xml.Linq;

namespace FPN.Bussines.Services
{
	internal class ImportService : IImportService
	{
		private readonly ILogger<ImportService> logger;
		private readonly IDataConverter transformer;

		public ImportService(ILogger<ImportService> logger, IDataConverter converter)
		{
			logger.LogDebug("Ctor");
			this.logger = logger;
			this.transformer = converter;
		}

		public IInvoice Import(string filePath)
		{
			logger.LogInformation("Import from file {filePath}", filePath);
			using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			return Import(fs);
		}

		private IInvoice Import(Stream input)
		{
			var document = XDocument.Load(input);
			var invoice = transformer.Transform(document);
			return invoice;
		}
	}
}