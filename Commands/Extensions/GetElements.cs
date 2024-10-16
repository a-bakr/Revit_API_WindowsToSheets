namespace WindowsToSheets.Commands.Extensions;

public static class GetElements
{
	public static FilteredElementCollector Collector(Document doc) => new FilteredElementCollector(doc);

	public static List<FamilySymbol> GetTitleBlocks(this Document doc)
	{
		return new FilteredElementCollector(doc)
			.OfCategory(BuiltInCategory.OST_TitleBlocks)
			.OfClass(typeof(FamilySymbol))
			.Cast<FamilySymbol>()
			.ToList();
	}

	public static FamilyInstance GetTitleBlock(this Document doc, ViewSheet sheet)
	{
		return new FilteredElementCollector(doc, sheet.Id)
			.OfCategory(BuiltInCategory.OST_TitleBlocks)
			.WhereElementIsNotElementType()
			.Cast<FamilyInstance>()
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