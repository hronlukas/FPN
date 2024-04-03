using FPN.Bussines.Data;
using FPN.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FPN.Views
{
	/// <summary>
	/// Interaction logic for NumberDetailsView.xaml
	/// </summary>
	public partial class NumberDetailsView : UserControl
	{
		public static readonly DependencyProperty InvoiceProperty =
			DependencyProperty.Register(nameof(Invoice), typeof(IInvoice), typeof(NumberDetailsView), new PropertyMetadata(OnInvoiceChanged));

		public static readonly DependencyProperty SelectedNumberProperty =
			DependencyProperty.Register(nameof(SelectedNumber), typeof(INumber), typeof(NumberDetailsView), new PropertyMetadata(OnSelectedNumberChanged));

		public NumberDetailsView()
		{
			InitializeComponent();
		}

		public IInvoice Invoice
		{
			get { return (IInvoice)GetValue(InvoiceProperty); }
			set { SetValue(InvoiceProperty, value); }
		}

		public INumber SelectedNumber
		{
			get { return (INumber)GetValue(SelectedNumberProperty); }
			set { SetValue(SelectedNumberProperty, value); }
		}

		private NumberDetailsViewModel ViewModel => DataContext as NumberDetailsViewModel ?? throw new System.Exception("Failed to get view model");

		private static void OnInvoiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is IInvoice i)
			{
				(d as NumberDetailsView)?.OnInvoiceChanged(i);
			}
		}

		private static void OnSelectedNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is INumber n)
			{
				(d as NumberDetailsView)?.OnSelectedNumberChanged(n);
			}
		}

		private void OnInvoiceChanged(IInvoice invoice)
		{
			if (SelectedNumber != null)
			{
				ViewModel.SetData(invoice, SelectedNumber);
			}
		}

		private void OnSelectedNumberChanged(INumber n)
		{
			if (Invoice != null)
			{
				ViewModel.SetData(Invoice, n);
			}
		}
	}
}