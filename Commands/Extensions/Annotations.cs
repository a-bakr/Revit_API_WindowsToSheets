namespace WindowsToSheets.Commands.Extensions
{
	public static class Annotations
	{
		public static void AddAnnotations(this Document doc, View view, FamilyInstance window)
		{
			// Example of adding a tag
			XYZ location = (window.Location as LocationPoint).Point;
			IndependentTag.Create(doc, view.Id, new Reference(window), false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, location);

			// Example of adding dimensions
			Line dimensionLine = Line.CreateBound(location + new XYZ(-1, 0, 0), location + new XYZ(1, 0, 0));
			var refA = new ReferenceArray();
			refA.Append(new Reference(window));
			Dimension dim = doc.Create.NewDimension(view, dimensionLine, refA);
		}
	}
}
