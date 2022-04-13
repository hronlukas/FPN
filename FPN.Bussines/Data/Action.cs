using System;

namespace FPN.Bussines.Data
{
	internal abstract class Action : IAction
	{
		public string TargetNo { get; set; }
		public DateTime StartDate { get; set; }
		public int Amount { get; set; }
		public IService Service { get; set; }
	}
}