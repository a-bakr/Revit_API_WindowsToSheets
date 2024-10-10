using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WindowsToSheets.Commands.Extensions;
using WindowsToSheets.Models;
using RelayCommand = WindowsToSheets.Commands.RelayCommand;

namespace WindowsToSheets.ViewModels
{
	public class WindowsToSheetsViewModel : INotifyPropertyChanged
	{
		private readonly Document _doc;

		public WindowsToSheetsViewModel(Document doc)
		{
			_doc = doc;

			// Initialize commands
			SelectWindowsCommand = new RelayCommand(SelectWindows);
			CreateViewsCommand = new RelayCommand(CreateViews);
			CreateSheetsCommand = new RelayCommand(CreateSheets);

			// Initialize collections
			AllWindows = new ObservableCollection<WindowModel>();
			SelectedWindows = new ObservableCollection<WindowModel>();
			TitleBlocks = new ObservableCollection<FamilySymbol>();
			ViewTemplates = new ObservableCollection<View>();

			// Load data
			LoadAllWindows();
			LoadTitleBlocks();
			LoadViewTemplates();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		// Windows Section
		public ObservableCollection<WindowModel> AllWindows { get; set; }
		public ObservableCollection<WindowModel> SelectedWindows { get; set; }
		public ICommand SelectWindowsCommand { get; set; }

		// View Section
		private string _viewName = "Win";
		public string ViewName
		{
			get => _viewName;
			set
			{
				_viewName = value;
				OnPropertyChanged(nameof(ViewName));
			}
		}

		private View _viewTemplate;
		public View ViewTemplate
		{
			get => _viewTemplate;
			set
			{
				_viewTemplate = value;
				OnPropertyChanged(nameof(ViewTemplate));
			}
		}

		private int _viewScale = 20;
		public int ViewScale
		{
			get => _viewScale;
			set
			{
				_viewScale = value;
				OnPropertyChanged(nameof(ViewScale));
			}
		}

		public ObservableCollection<View> ViewTemplates { get; set; }
		public ICommand CreateViewsCommand { get; set; }

		// Sheet Section
		private string _sheetName = "WindowsSheet";
		public string SheetName
		{
			get => _sheetName;
			set
			{
				_sheetName = value;
				OnPropertyChanged(nameof(SheetName));
			}
		}

		private FamilySymbol _selectedTitleBlock;
		public FamilySymbol SelectedTitleBlock
		{
			get => _selectedTitleBlock;
			set
			{
				_selectedTitleBlock = value;
				OnPropertyChanged(nameof(SelectedTitleBlock));
			}
		}

		public ObservableCollection<FamilySymbol> TitleBlocks { get; set; }
		public ICommand CreateSheetsCommand { get; set; }

		private void LoadAllWindows()
		{
			var allWindows = _doc.GetAllBuiltInFacedWindows();
			var groupedWindows = allWindows.GroupWindowsByType();
			var windows = groupedWindows.Select(x => x.Value.FirstOrDefault()).Where(x => x != null).ToList();

			foreach (var window in windows)
			{
				AllWindows.Add(new WindowModel(window));
			}
		}

		private void LoadTitleBlocks()
		{
			var collector = new FilteredElementCollector(_doc);
			var titleBlocks = collector.OfCategory(BuiltInCategory.OST_TitleBlocks)
																 .OfClass(typeof(FamilySymbol))
																 .Cast<FamilySymbol>()
																 .ToList();

			foreach (var tb in titleBlocks)
			{
				TitleBlocks.Add(tb);
			}

			_selectedTitleBlock = TitleBlocks.FirstOrDefault();
		}

		private void LoadViewTemplates()
		{
			var templates = _doc.GetAllViewTemplates();
			foreach (var template in templates)
			{
				ViewTemplates.Add(template);
			}
			_viewTemplate = ViewTemplates.FirstOrDefault();
		}

		private void SelectWindows(object parameter)
		{
			SelectedWindows.Clear();
			foreach (var window in AllWindows)
			{
				SelectedWindows.Add(window);
			}
		}

		private void CreateViews(object parameter)
		{
			if (SelectedWindows.Count == 0)
				return;

			using (var tran = new Transaction(_doc, "Creating Views"))
			{
				tran.Start();

				foreach (var windowModel in SelectedWindows)
				{
					var window = windowModel.WindowInstance;

					// Create a view for the window
					var view = window.CreateWindowView(_doc, ViewScale);

					// Set view properties
					if (!string.IsNullOrEmpty(ViewName))
						view.Name = ViewName + "_" + view.Name;

					ViewTemplate.Scale = ViewScale;
					if (ViewTemplate != null)
						view.ViewTemplateId = ViewTemplate.Id;

					// Store the created view in the model
					windowModel.AssociatedView = view;
				}

				tran.Commit();
			}
		}

		private void CreateSheets(object parameter)
		{
			if (SelectedWindows.Count == 0 || SelectedTitleBlock == null)
				return;

			using (Transaction tran = new Transaction(_doc, "Creating Sheets"))
			{
				tran.Start();

				var sheet = ViewSheet.Create(_doc, SelectedTitleBlock.Id);

				if (!string.IsNullOrEmpty(SheetName))
				{
					sheet.Name = SheetName;
				}

				var sheetDim = SelectedTitleBlock.BoundingBox(sheet);
				var origin = sheetDim.Min;
				origin += new XYZ(0.3, 0.3, 0);

				foreach (var windowModel in SelectedWindows)
				{
					var view = windowModel.AssociatedView;

					if (view != null)
					{
						var viewPort = view.PlaceOnSheet(_doc, sheet, origin);

						var viewDim = viewPort.BoundingBox(sheet);
						var y = origin.Y + viewDim.Height + 1.0 / 12;
						origin = new XYZ(origin.X, y, origin.Z);
					}
				}

				tran.Commit();
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
