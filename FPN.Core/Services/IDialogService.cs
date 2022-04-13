namespace FPN.Core.Services
{
	public interface IDialogService
	{
		string ShowOpenFileDialog(string filter, string? title = null);
	}
}