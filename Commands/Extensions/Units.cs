namespace WindowsToSheets.Commands.Extensions
{
	public static class Units
	{
		private const double _inchToMm = 25.4;
		private const double _footToMm = 12 * _inchToMm;
		private const double _footToMeter = _footToMm * 0.001;
		private const double _sqfToSqm = _footToMeter * _footToMeter;
		private const double _cubicFootToCubicMeter = _footToMeter * _sqfToSqm;

		public static double FootToMm(this double length)
		{
			return length * _footToMm;
		}

		public static int FootToMmInt(this double length)
		{
			return (int)Math.Round(_footToMm * length,
				MidpointRounding.AwayFromZero);
		}

		public static double FootToCm(this double length)
		{
			return FootToMetre(length) * 100;
		}

		public static double FootToMetre(this double length)
		{
			return length * _footToMeter;
		}

		public static double MmToFoot(this double length)
		{
			return length / _footToMm;
		}

		public static XYZ MmToFoot(XYZ v)
		{
			return v.Divide(_footToMm);
		}

		public static double CubicFootToCubicMeter(this double volume)
		{
			return volume * _cubicFootToCubicMeter;
		}
	}
}
