namespace WindowsToSheets.Commands.Extensions
{
	public static class Sheets
	{
		public static void PlaceViewsOnSheets(this Document doc, List<View> views)
		{
			var sheetName = "A0 Metric";
			var titleBlock = doc.GetTitleBlockSymbol(sheetName);
			var sheet = ViewSheet.Create(doc, titleBlock.Id);
			sheet.Name = "Window Elevations";

			var sheetDim = titleBlock.BoundingBox(sheet);
			//var firstViewDim = views.FirstOrDefault().BoundingBox(sheet);
			var origin = sheetDim.Min;
			origin += new XYZ(0.3, 0.3, 0);

			foreach (var view in views)
			{
				var viewport = Viewport.Create(doc, sheet.Id, view.Id, origin);
				var viewDim = viewport.BoundingBox(sheet);

				var y = origin.Y + viewDim.Height + 1.0 / 12;
				origin = new XYZ(origin.X, y, origin.Z);
			}
		}

		public static Viewport PlaceOnSheet(this View view, Document doc, ViewSheet sheet, XYZ point)
		{
			return Viewport.Create(doc, sheet.Id, view.Id, point);
		}
	}
}
