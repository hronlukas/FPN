using FPN.Bussines.Data;
using FPN.Core;
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
		private readonly InvoiceViewModel invoiceViewModel;

		public static readonly DependencyProperty InvoiceProperty =
			DependencyProperty.Register(nameof(Invoice), typeof(IInvoice), typeof(InvoiceView), new PropertyMetadata(OnInvoiceChanged));

		public InvoiceView()
		{
			InitializeComponent();
			invoiceViewModel = ServiceLocator.Current.GetRequiredService<InvoiceViewModel>();
			DockPanel.DataContext = invoiceViewModel;
		}

		public IInvoice Invoice
		{
			get { return (IInvoice)GetValue(InvoiceProperty); }
			set { SetValue(InvoiceProperty, value); }
		}

		private static void OnInvoiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is IInvoice i)
			{
				(d as InvoiceView)?.OnInvoiceChanged(i);
			}
		}

		private void OnInvoiceChanged(IInvoice invoice)
		{
			invoiceViewModel.SetInvoice(invoice);
		}
	}
}