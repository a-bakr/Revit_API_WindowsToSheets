using Autodesk.Revit.UI;
using System.Windows;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace Gene.Commands.Extensions
{
	public abstract class RevitCommand : IExternalCommand
	{
		public abstract void Action();
		public UIDocument UIDoc { get; set; }
		public UIApplication UIApp { get; set; }
		public Autodesk.Revit.ApplicationServices.Application App { get; set; }
		public Document Doc { get; set; }

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApp = commandData.Application;
			UIDoc = UIApp.ActiveUIDocument;
			App = UIApp.Application;
			Doc = UIDoc.Document;

			try
			{
				Action();
			}
			catch (OperationCanceledException) { }
			catch (Exception e)
			{
				MessageBox.Show($"Error: {e.Message}", "Warning", MessageBoxButton.OK);
				return Result.Cancelled;
			}

			return Result.Succeeded;
		}
	}
}