using FPN.Bussines.Data;
using FPN.Bussines.Services;
using FPN.Core.Mvvm;
using FPN.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace FPN.ViewModels
{
	internal class MainWindowViewModel : ViewModelBase
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

		private void OnImport()
		{
			var filePath = dialogService.ShowOpenFileDialog("XML Files|*.xml|All Files|*.*");
			if (!string.IsNullOrEmpty(filePath))
			{
				Import(filePath);
			}
		}

		public ICommand ImportCommand { get; }

		public string ErrorMessage { get; set; } = string.Empty;

		public IInvoice? SelectedInvoice { get; set; }

		internal void Import(string filePath)
		{
			try
			{
				ErrorMessage = string.Empty;
				SelectedInvoice = importService.Import(filePath);
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