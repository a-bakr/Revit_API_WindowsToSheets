namespace WindowsToSheets.Commands.Extensions
{
	public static class ElementUtils
	{
		public static (double Width, double Height, XYZ Min, XYZ Max) BoundingBox(this Element viewport, View view)
		{
			var viewBounding = viewport.get_BoundingBox(view);
			var viewHeight = viewBounding.Max.Y - viewBounding.Min.Y;
			var viewWidth = viewBounding.Max.X - viewBounding.Min.X;
			return (viewWidth, viewHeight, viewBounding.Min, viewBounding.Max);
		}

		public static ElementFilter FiltersElementByCategory(
				this Document doc, IEnumerable<string> categoryNames)
		{

			ElementFilter categoriesFilter = null;
			if (categoryNames == null || !categoryNames.Any())
			{
				var categoryIds = new List<ElementId>();
				foreach (var categoryName in categoryNames)
				{
					var category = doc.Settings.Categories.get_Item(categoryName);
					if (category != null)
						categoryIds.Add(category.Id);
				}

				var categoryFilters = new List<ElementFilter>();
				if (categoryIds.Count > 0)
				{
					var categoryRule = new FilterCategoryRule(categoryIds);
					var categoryFilter = new ElementParameterFilter(categoryRule);
					categoryFilters.Add(categoryFilter);
				}

				if (categoryFilters.Count > 0)
					categoriesFilter = new LogicalOrFilter(categoryFilters);
			}

			return categoriesFilter;
		}

		public static XYZ RayIntersect(this Element element, List<string> categories, XYZ direction)
		{
			// Find a 3D view to use for the reference Intersector constructor
			var collector = new FilteredElementCollector(element.Document);
			bool IsNotTemplate(View3D v3) => !v3.IsTemplate;
			var view3D = collector
					.OfClass(typeof(View3D))
					.Cast<View3D>()
					.First(IsNotTemplate);

			// Use the center of the skylight bounding box as the start point.
			var box = element.get_BoundingBox(view3D);
			var center = box.Min.Add(box.Max).Multiply(0.5);

			// Project in the negative Z direction down to the ceiling.
			var elementFilter = element.Document.FiltersElementByCategory(categories);
			var refIntersector = new ReferenceIntersector(
					elementFilter, FindReferenceTarget.Face, view3D);
			refIntersector.FindReferencesInRevitLinks = true;
			var referenceWithContext = refIntersector.FindNearest(center, direction);
			var reference = referenceWithContext?.GetReference();
			return reference?.GlobalPoint;
		}

		public static void Rotate(this Element elem, XYZ lcPoint, double angle)
		{
			using (SubTransaction subTrans = new SubTransaction(elem.Document))
			{
				subTrans.Start();
				var pt = new XYZ(lcPoint.X, lcPoint.Y, lcPoint.Z + 10000);
				var line = Line.CreateBound(lcPoint, pt);
				ElementTransformUtils.RotateElement(
					elem.Document, elem.Id, line, angle * Math.PI / 180);
				subTrans.Commit();
			}
		}
	}
}