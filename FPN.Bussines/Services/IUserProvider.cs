using FPN.Bussines.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPN.Bussines.Services
{
	public interface IUserProvider
	{
		IUser GetUserByPhone(string number);

		IEnumerable<IUser> GetUsers();
	}

	public interface IUserEditor
	{
		Task Save();
	}

	internal class UserProvider : IUserProvider, IUserEditor
	{
		private readonly IList<IUser> users;
		private readonly IUserStorage storage;

		public UserProvider(IUserStorage storage)
		{
			this.storage = storage;
			users = storage.Load().GetAwaiter().GetResult().ToList();
		}

		public IUser GetUserByPhone(string number)
		{
			var user = users.FirstOrDefault(x => x.Number == number);
			if (user is null)
			{
				// Unknown user
				user = new User { Number = number, Name = number, };
			}

			return user;
		}

		public IEnumerable<IUser> GetUsers()
		{
			return users;
		}

		public async Task Save()
		{
			await storage.Save(users);
		}
	}
}