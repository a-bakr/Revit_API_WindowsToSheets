namespace WindowsToSheets.Models
{
	public class WindowModel(FamilyInstance windowInstance)
	{
		public FamilyInstance WindowInstance { get; set; } = windowInstance;
		public View AssociatedView { get; set; }
		public string Name => WindowInstance.Name;
	}
}