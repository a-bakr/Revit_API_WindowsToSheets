namespace WindowsToSheets.Commands.Extensions
{
	public static class DocumentExtensions
	{
		//public static IList<FamilyInstance> GetAllBuiltInFacedWindows(this Document doc)
		//{
		//	var collector = new FilteredElementCollector(doc)
		//		.OfCategory(BuiltInCategory.OST_Windows)
		//		.OfClass(typeof(FamilyInstance))
		//		.Cast<FamilyInstance>()
		//		.ToList();

		//	return collector;
		//}

		//public static Dictionary<ElementId, List<FamilyInstance>> GroupWindowsByType(this IEnumerable<FamilyInstance> windows)
		//{
		//	return windows.GroupBy(w => w.Symbol.Id)
		//		.ToDictionary(g => g.Key, g => g.ToList());
		//}

		//public static View CreateWindowView(this Document doc, FamilyInstance window)
		//{
		//	ViewFamilyType viewFamilyType = new FilteredElementCollector(doc)
		//		.OfClass(typeof(ViewFamilyType))
		//		.Cast<ViewFamilyType>()
		//		.FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

		//	var view = View3D.CreateIsometric(doc, viewFamilyType.Id);
		//	view.Name = $"Window View - {window.Name}";

		//	return view;
		//}

		public static List<View> GetAllViewTemplates(this Document doc)
		{
			var collector = new FilteredElementCollector(doc)
				.OfClass(typeof(View))
				.Cast<View>()
				.Where(v => v.IsTemplate)
				.ToList();

			return collector;
		}
	}
}