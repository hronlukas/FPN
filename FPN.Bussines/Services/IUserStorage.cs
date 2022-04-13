using FPN.Bussines.Data;
using System.Collections.Generic;

namespace FPN.Bussines.Services
{
	internal interface IUserStorage
	{
		IEnumerable<IUser> Load();

		void Save(IEnumerable<IUser> users);
	}
}