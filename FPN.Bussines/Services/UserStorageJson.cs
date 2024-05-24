using FPN.Bussines.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json;

namespace FPN.Bussines.Services
{
	internal class UserStorageJson : IUserStorage
	{
		private const string filePath = "users.json";

		public async Task<IEnumerable<IUser>> Load()
		{
			if (!File.Exists(filePath))
			{
				return Enumerable.Empty<IUser>();
			}

			using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			return await JsonSerializer.DeserializeAsync<List<User>>(fs);
		}

		public async Task Save(IEnumerable<IUser> users)
		{
			using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			await JsonSerializer.SerializeAsync(fs, users.Cast<User>().ToList());
		}
	}
}