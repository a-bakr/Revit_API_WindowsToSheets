using System.Windows;
using WindowsToSheets.ViewModels;

namespace WindowsToSheets.Views
{
	public partial class WindowsToSheetsView : Window
	{
		public WindowsToSheetsView(WindowsToSheetsViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}