using FPN.Bussines.Data;
using System.Xml.Linq;

namespace FPN.Bussines.Services
{
	internal interface IDataConverter
	{
		IInvoice Transform(XDocument input);
	}
}