using FPN.Bussines.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utf8Json;

namespace FPN.Bussines.Services
{
	internal class UserStorageJson : IUserStorage
	{
		private const string filePath = "users.json";

		public IEnumerable<IUser> Load()
		{
			if (!File.Exists(filePath))
			{
				return Enumerable.Empty<IUser>();
			}

			using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			return JsonSerializer.Deserialize<List<User>>(fs);
		}

		public void Save(IEnumerable<IUser> users)
		{
			using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			JsonSerializer.Serialize(fs, users.Cast<User>().ToList());
		}
	}
}