using FPN.Bussines.Data;
using System.Threading.Tasks;

namespace FPN.Bussines.Services
{
	public interface IImportService
	{
		Task<IInvoice> Import(string filePath);
	}
}