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
	}
}