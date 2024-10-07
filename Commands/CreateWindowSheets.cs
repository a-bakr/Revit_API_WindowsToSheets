using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using WindowsToSheets.Commands.Extensions;

namespace WindowsToSheets.Commands
{
	[Transaction(TransactionMode.Manual)]
	public class CreateWindowSheets : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			Document doc = uiapp.ActiveUIDocument.Document;


			try
			{
				// Retrieve all built-in faced windows
				List<FamilyInstance> allWindows = doc.GetAllBuiltInFacedWindows();

				// Group windows by unique types
				var groupedWindows = allWindows.GroupWindowsByType();

				// Get one window from Each group
				//var windows = groupedWindows.

				// Create views and annotate them
				List<View> views = new List<View>();
				foreach (var windowGroup in groupedWindows)
				{
					View view = doc.CreateWindowView(windowGroup.Value.FirstOrDefault());
					views.Add(view);
				}

				// Place the views on sheets
				doc.PlaceViewsOnSheets(views);

				return Result.Succeeded;
			}
			catch (Exception e)
			{
				message = e.Message;
				return Result.Failed;
			}
		}
	}
}