using CommonServiceLocator;
using FPN.ViewModels;
using System.Windows;

namespace FPN.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				// Note that you can have more than one file.
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				// Assuming you have one file that you care about, pass it off to whatever
				// handling code you have defined.
				(DataContext as MainWindowViewModel)?.Import(files[0]);
			}
		}
	}
}