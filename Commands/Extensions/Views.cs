namespace WindowsToSheets.Commands.Extensions
{
	public static class Views
	{
		public static View CreateWindowView(this Document doc, FamilyInstance window)
		{
			using (var tx = new Transaction(doc, "Create Annotated Window View"))
			{
				tx.Start();

				// Get the window's location
				var location = (window.Location as LocationPoint).Point;

				// Retrieve the ViewFamilyType for Elevation views
				var viewFamilyType = new FilteredElementCollector(doc)
						.OfClass(typeof(ViewFamilyType))
						.Cast<ViewFamilyType>()
						.FirstOrDefault(vft => vft.ViewFamily == ViewFamily.Elevation);

				// Ensure the ViewFamilyType was found
				if (viewFamilyType == null)
				{
					tx.RollBack();
					throw new System.Exception("Elevation view family type not found.");
				}

				// Create an elevation marker at the window's location
				var marker = ElevationMarker.CreateElevationMarker(doc, viewFamilyType.Id, location, 20);

				// Create the elevation view using the marker
				var elevationView = marker.CreateElevation(doc, doc.ActiveView.Id, 0);

				// Adjust the view's crop region based on the window's bounding box
				var bbox = window.get_BoundingBox(elevationView);

				// Add padding around the window
				var padding = 0.2; // Adjust the padding as needed
				bbox.Max = new XYZ(bbox.Max.X + padding, bbox.Max.Z + padding, bbox.Max.Y + padding);
				bbox.Min = new XYZ(bbox.Min.X - padding, bbox.Min.Z - padding, bbox.Min.Y - padding);

				elevationView.CropBox = bbox;
				elevationView.CropBoxActive = true;

				tx.Commit();

				return elevationView;
			}
		}
	}
}
