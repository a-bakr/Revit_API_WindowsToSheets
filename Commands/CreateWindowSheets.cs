using Autodesk.Revit.Attributes;
using Gene.Commands.Extensions;
using WindowsToSheets.Commands.Extensions;

namespace WindowsToSheets.Commands
{
	[Transaction(TransactionMode.Manual)]
	public class CreateWindowSheets : RevitCommand
	{
		public override void Action()
		{
			// Retrieve all built-in faced windows
			var allWindows = Doc.GetAllBuiltInFacedWindows();

			// Group windows by unique types
			var groupedWindows = allWindows.GroupWindowsByType();

			// Get one window from Each group
			var windows = groupedWindows
				.Select(x => x.Value.FirstOrDefault()).Where(x => x != null);

			// Create views and annotate them
			var views = windows.Select(window => Doc.CreateWindowView(window));

			// Place the views on sheets
			Doc.PlaceViewsOnSheets(views);
		}
	}
}