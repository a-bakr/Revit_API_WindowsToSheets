using WindowsToSheets.ViewModels;

namespace WindowsToSheets.Views
{
	public sealed partial class WindowsToSheetsView
	{
		public WindowsToSheetsView(WindowsToSheetsViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}

		private void SetViewName(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void PrintView(object sender, System.Windows.RoutedEventArgs e)
		{

		}
	}
}