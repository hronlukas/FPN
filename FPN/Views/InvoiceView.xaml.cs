using FPN.Bussines.Data;
using FPN.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FPN.Views
{
	/// <summary>
	/// Interaction logic for InvoiceView.xaml
	/// </summary>
	public partial class InvoiceView : UserControl
	{
		public static readonly DependencyProperty InvoiceProperty =
			DependencyProperty.Register(nameof(Invoice), typeof(IInvoice), typeof(InvoiceView), new PropertyMetadata(OnInvoiceChanged));

		public InvoiceView()
		{
			InitializeComponent();
		}

		public IInvoice Invoice
		{
			get { return (IInvoice)GetValue(InvoiceProperty); }
			set { SetValue(InvoiceProperty, value); }
		}

		private InvoiceViewModel ViewModel => DataContext as InvoiceViewModel ?? throw new System.Exception("Failed to get view model");

		private static void OnInvoiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is IInvoice i)
			{
				(d as InvoiceView)?.OnInvoiceChanged(i);
			}
		}

		private void OnInvoiceChanged(IInvoice invoice)
		{
			ViewModel.SetInvoice(invoice);
		}
	}
}