using Microsoft.Win32;

namespace FPN.Core.Services
{
	public class DialogService : IDialogService
	{
		public string ShowOpenFileDialog(string filter, string? title)
		{
			var ofd = new OpenFileDialog
			{
				Filter = filter,
				Multiselect = false,
				Title = title ?? "Open file",
			};

			var result = ofd.ShowDialog();
			if (result.HasValue && result.Value)
			{
				return ofd.FileName;
			}

			return string.Empty;
		}
	}
}