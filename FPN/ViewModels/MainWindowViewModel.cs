using CommunityToolkit.Mvvm.Input;
using FPN.Bussines.Data;
using FPN.Bussines.Services;
using FPN.Core.Mvvm;
using FPN.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FPN.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly ILogger logger;
		private readonly IImportService importService;
		private readonly IDialogService dialogService;

		public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IImportService importService, IDialogService dialogService)
		{
			this.logger = logger;
			this.importService = importService;
			this.dialogService = dialogService;

			// Commands
			ImportCommand = new RelayCommand(OnImport);
		}

		private async void OnImport()
		{
			var filePath = dialogService.ShowOpenFileDialog("XML Files|*.xml|All Files|*.*");
			if (!string.IsNullOrEmpty(filePath))
			{
				await Import(filePath);
			}
		}

		public ICommand ImportCommand { get; }

		public string ErrorMessage { get; set; } = string.Empty;

		public IInvoice? SelectedInvoice { get; set; }

		internal async Task Import(string filePath)
		{
			try
			{
				ErrorMessage = string.Empty;
				SelectedInvoice = await importService.Import(filePath);
				OnPropertyChanged(nameof(SelectedInvoice));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Invoice import failed, '{filePath}'", filePath);
				ErrorMessage = "Invoice import failed";
			}
		}
	}
}