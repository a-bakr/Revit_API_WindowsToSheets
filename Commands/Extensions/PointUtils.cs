﻿namespace WindowsToSheets.Commands.Extensions
{
	public static class PointUtils
	{
		public static bool IsEqual(this XYZ _xyz, XYZ _another)
		{
			return true
									 && _xyz.X.IsEqual(_another.X)
									 && _xyz.Y.IsEqual(_another.Y)
									 && _xyz.Z.IsEqual(_another.Z);
		}
	}
}