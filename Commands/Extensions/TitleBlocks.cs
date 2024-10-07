namespace WindowsToSheets.Commands.Extensions
{
	public static class TitleBlocks
	{
		public static FamilySymbol GetTitleBlockSymbol(this Document doc, string titleBlockName)
		{
			// Filter for title block family symbols
			var collector = new FilteredElementCollector(doc)

				.OfCategory(BuiltInCategory.OST_TitleBlocks)
				.OfClass(typeof(FamilySymbol))
				.Cast<FamilySymbol>();

			// Find the title block with the given name
			var titleBlockSymbol = collector
				.FirstOrDefault(symbol => symbol.Name.Equals(titleBlockName, StringComparison.InvariantCultureIgnoreCase));

			if (titleBlockSymbol == null)
				titleBlockSymbol = collector.FirstOrDefault();

			return titleBlockSymbol;
		}
	}
}
