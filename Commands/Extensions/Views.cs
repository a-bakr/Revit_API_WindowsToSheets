namespace WindowsToSheets.Commands.Extensions
{
	public static class Views
	{
		public static View CreateWindowView(this FamilyInstance window, Document doc, int viewScale)
		{
			// Get the window's location
			var location = (window.Location as LocationPoint)?.Point;

			// Find the ViewFamilyType for Elevations
			var elevationViewType = new FilteredElementCollector(doc)
				.OfClass(typeof(ViewFamilyType))
				.Cast<ViewFamilyType>()
				.FirstOrDefault(vft => vft.ViewFamily == ViewFamily.Elevation);

			// Create the elevation marker at the window's position
			var elevationMarker = ElevationMarker
				.CreateElevationMarker(doc, elevationViewType?.Id, location, viewScale);

			// Create an elevation view using the marker, index 0 corresponds to teh first view created
			var elevationView = elevationMarker.CreateElevation(doc, doc.ActiveView.Id, 0);
			window.CorrectViewDirection(doc, elevationMarker);

			// Set the view properties to make it an elevation
			elevationView.CropBoxActive = true;
			elevationView.CropBoxVisible = false;

			// Set the crop region based on the bounding box of the window
			var windowBoundingBox = window.get_BoundingBox(elevationView);
			var min = windowBoundingBox.Min;
			var max = windowBoundingBox.Max;

			// Add a buffer around the window to ensure the entire element is visible
			var cropBox = elevationView.CropBox;
			var padding = 0.3;
			cropBox.Min = new XYZ(min.X - padding, min.Z - padding, min.Z - padding);
			cropBox.Max = new XYZ(max.X + padding, max.Z + padding, max.Z + padding);
			elevationView.CropBox = cropBox;

			elevationView.Name = window.Name;
			// Optionally set the detail level and scale of the view
			elevationView.get_Parameter(BuiltInParameter.VIEW_DETAIL_LEVEL).Set((int)ViewDetailLevel.Fine);
			return elevationView;
		}

		private static void CorrectViewDirection(this FamilyInstance window, Document doc, ElevationMarker elevationMarker)
		{
			var location = (window.Location as LocationPoint)?.Point;
			var facingDirection = window.FacingOrientation;

			// Get the active view's right direction to align the elevation view
			var rightDirection = doc.ActiveView.RightDirection;

			// Calculate the angle between the view's right direction and the window's facing direction
			var angle = rightDirection.AngleTo(facingDirection);
			angle -= Math.PI / 2;

			// Rotate the elevation marker to align with the window's facing direction
			Line axis = Line.CreateBound(location, location?.Add(XYZ.BasisZ)); // Rotation axis (vertical axis through the window)
			ElementTransformUtils.RotateElement(doc, elevationMarker.Id, axis, angle);
		}
	}
}