using System;

namespace FPN.Bussines.Data
{
	public interface IAction
	{
		string TargetNo { get; }
		DateTime StartDate { get; }
		int Amount { get; }
		IService Service { get; }
	}
}