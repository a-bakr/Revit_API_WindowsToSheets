using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using WindowsToSheets.ViewModels;
using WindowsToSheets.Views;

namespace WindowsToSheets.Commands
{
	[Transaction(TransactionMode.Manual)]
	public class CreateWindowSheets : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			var doc = commandData.Application.ActiveUIDocument.Document;

			var viewModel = new WindowsToSheetsViewModel(doc);
			var view = new WindowsToSheetsView(viewModel);
			view.ShowDialog();

			return Result.Succeeded;
		}
	}
}