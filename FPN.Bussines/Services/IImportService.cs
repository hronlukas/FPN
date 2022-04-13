using FPN.Bussines.Data;

namespace FPN.Bussines.Services
{
	public interface IImportService
	{
		IInvoice Import(string filePath);
	}
}