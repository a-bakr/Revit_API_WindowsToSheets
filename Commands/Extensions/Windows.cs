namespace WindowsToSheets.Commands.Extensions
{
	public static class Windows
	{

		public static List<FamilyInstance> GetAllBuiltInFacedWindows(this Document doc)
		{
			// Filter to only get built-in faced windows
			var collector = new FilteredElementCollector(doc);
			var windows = collector.OfClass(typeof(FamilyInstance))
				.OfCategory(BuiltInCategory.OST_Windows)
				.WhereElementIsNotElementType()
				.Cast<FamilyInstance>()
				//.Where(w => IsBuiltInFacedWindow(w))
				.ToList();
			return windows;
		}

		public static bool IsBuiltInFacedWindow(this FamilyInstance window)
		{
			// Logic to determine if the window is built-in faced
			// This could involve checking specific parameters or categories
			return window.Symbol.FamilyName.Contains("Built-in Faced");
		}

		public static Dictionary<ElementId, List<FamilyInstance>> GroupWindowsByType(this List<FamilyInstance> windows)
		{
			return windows.GroupBy(w => w.GetTypeId())
				.ToDictionary(g => g.Key, g => g.ToList());
		}
	}
}
