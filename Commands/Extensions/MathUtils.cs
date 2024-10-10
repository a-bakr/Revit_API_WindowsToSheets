namespace WindowsToSheets.Commands.Extensions
{
	public static class MathUtils
	{
		public const double Pre = 1.0e-5;

		public static bool IsEqual(this double _d1, double _d2)
		{
			return Math.Abs(_d1 - _d2) < Pre;
		}
	}
}