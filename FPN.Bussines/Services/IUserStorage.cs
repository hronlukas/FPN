using FPN.Bussines.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPN.Bussines.Services
{
	internal interface IUserStorage
	{
		Task<IEnumerable<IUser>> Load();

		Task Save(IEnumerable<IUser> users);
	}
}