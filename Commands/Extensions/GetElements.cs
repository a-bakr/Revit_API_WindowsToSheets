namespace WindowsToSheets.Commands.Extensions;

public static class GetElements
{
	public static FilteredElementCollector Collector;
	public static List<FamilySymbol> GetTitleBlocks(this Document doc)
	{
		return new FilteredElementCollector(doc)
			.OfCategory(BuiltInCategory.OST_TitleBlocks)
			.OfClass(typeof(FamilySymbol))
			.Cast<FamilySymbol>()
			.ToList();
	}
	public static FamilySymbol GetTitleBlocks(this Document doc, ViewSheet sheet)
	{
		return new FilteredElementCollector(doc, sheet.Id)
			.OfCategory(BuiltInCategory.OST_TitleBlocks)
			.WhereElementIsNotElementType()
			.Cast<FamilySymbol>()
			.FirstOrDefault();
	}

	public static List<View> GetViewTemplates(this Document doc)
	{
		return new FilteredElementCollector(doc)
			.OfClass(typeof(View))
			.Cast<View>()
			.Where(v => v.IsTemplate)
			.ToList();
	}

	public static List<FamilyInstance> GetWindows(this Document doc)
	{
		return new FilteredElementCollector(doc)
			.OfClass(typeof(FamilyInstance))
			.OfCategory(BuiltInCategory.OST_Windows)
			.WhereElementIsNotElementType()
			.Cast<FamilyInstance>()
			.ToList();
	}
}