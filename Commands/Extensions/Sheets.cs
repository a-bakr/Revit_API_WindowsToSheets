namespace WindowsToSheets.Commands.Extensions
{
	public static class Sheets
	{
		public static Viewport PlaceView(this ViewSheet sheet, Document doc, View view, XYZ point)
		{
			return Viewport.Create(doc, sheet.Id, view.Id, point);
		}

		public static void PlaceViews(this ViewSheet sheet, Document doc, IEnumerable<View> views)
		{
			var titleBlock = doc.GetTitleBlock(sheet);
			var sheetDim = titleBlock.BoundingBox(sheet);
			var origin = sheetDim.Min;
			origin += new XYZ(0.3, 0.3, 0);

			foreach (var view in views)
			{
				if (view == null) continue;
				var viewPort = sheet.PlaceView(doc, view, origin);
				var viewDim = viewPort.BoundingBox(sheet);
				var y = origin.Y + viewDim.Height + 1.0 / 12;
				origin = new XYZ(origin.X, y, origin.Z);
			}
		}
	}
}
