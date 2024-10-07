using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using WindowsToSheets.Commands;

namespace WindowsToSheets
{
	[UsedImplicitly]
	public class Application : ExternalApplication
	{
		public override void OnStartup()
		{
			CreateRibbon();
		}

		private void CreateRibbon()
		{

			var panel = Application.CreatePanel("WindowsToSheets");
			var path = GetAssemblyPath() + "/Resources/Icons";

			panel.AddPushButton<CreateWindowSheets>("CreateWindowSheets")
					.SetImage($"{path}/WindowIcon16.png")
					.SetLargeImage($"{path}/WindowIcon32.png");
		}

		private static string GetAssemblyPath()
		{
			var assemblyLocation = Assembly.GetExecutingAssembly().Location;
			var path = Path.GetDirectoryName(assemblyLocation);
			return path;
		}
	}
}