using Autodesk.Revit.UI.Selection;

namespace WindowsToSheets.Commands.Extensions
{
	internal class WallSelectionFilter : ISelectionFilter
	{
		public bool AllowElement(Element elem)
		{
			var elemCategoryId = elem?.Category?.Id?.Value ?? -1;
			BuiltInCategory builtInCategory = (BuiltInCategory)elemCategoryId;
			return builtInCategory == BuiltInCategory.OST_Walls;
		}

		public bool AllowReference(Reference reference, XYZ position)
		{
			return false;
		}
	}
}
