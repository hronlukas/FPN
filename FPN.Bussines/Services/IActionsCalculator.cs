using FPN.Bussines.Data;
using System.Collections.Generic;

namespace FPN.Bussines.Services
{
	public interface IActionsCalculator
	{
		/// <summary>
		/// Returns internal calls in the private network for given <paramref name="number"/>.
		/// </summary>
		/// <param name="number">The number to filter.</param>
		/// <returns></returns>
		IEnumerable<ICall> GetFpnCalls(INumber number);

		IEnumerable<ISms> GetFpnSms(INumber number);

		int GetFreeSecondsAmount();

		int GetFreeSmsAmount();

		/// <summary>
		/// Returns international calls (outside the Czech republic).
		/// </summary>
		/// <param name="number">The number to filter.</param>
		/// <returns></returns>
		IEnumerable<ICall> GetInterNationalCalls(INumber number);

		/// <summary>
		/// Returns national calls (in the Czech republic) except of FPN calls.
		/// </summary>
		/// <param name="number">The number to filter.</param>
		/// <returns></returns>
		IEnumerable<ICall> GetNationalCalls(INumber number);

		IEnumerable<ISms> GetNationalSms(INumber number);

		IEnumerable<INumber> GetNumbersWithSharedTariff();
	}
}