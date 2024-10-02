namespace WindowsToSheets.Commands.Extensions
{
	public static class Sheets
	{
		public static void PlaceViewsOnSheets(this Document doc, List<View> views)
		{
			using (var tx = new Transaction(doc, "Place Views on Sheets"))
			{
				tx.Start();

				var sheetName = "A0 Metric";
				var titleBlock = doc.GetTitleBlockSymbol(sheetName);
				var sheet = ViewSheet.Create(doc, titleBlock.Id);
				sheet.Name = "Window Elevations";

				var origin = sheet.Origin + new XYZ(0, 0, 0);
				var sheetHeight = sheet.GetParameter(BuiltInParameter.SHEET_HEIGHT).AsDouble();
				var sheetWidth = sheet.GetParameter(BuiltInParameter.SHEET_WIDTH).AsDouble();

				foreach (var view in views)
				{
					var viewport = Viewport.Create(doc, sheet.Id, view.Id, origin);

					// Update origin for next view placement
					var viewBounding = viewport.get_BoundingBox(sheet);
					var viewHeight = viewBounding.Max.Y;
					var viewWidth = viewBounding.Max.X;

					//double viewHeight = view.get_BoundingBox(sheet).Max.Y;
					origin = new XYZ(origin.X, origin.Y + sheetHeight, origin.Z);
				}

				tx.Commit();
			}
		}
	}
}
