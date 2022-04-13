using System;

namespace FPN.Bussines.Data
{
	public class Service : IService
	{
		public int Id1 { get; set; }
		public int Id2 { get; set; }
		public int Id3 { get; set; }
		public int Id4 { get; set; }

		public string Description1 { get; set; }
		public string Description2 { get; set; }
		public string Description3 { get; set; }
		public string Description4 { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is Service s)
			{
				return s.Id1 == Id1 && s.Id2 == Id2 && s.Id3 == Id3 && s.Id4 == Id4;
			}

			return false;
		}

		public override int GetHashCode() => HashCode.Combine(Id1, Id2, Id3, Id4);
	}
}