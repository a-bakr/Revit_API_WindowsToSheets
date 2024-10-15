using System.Collections.ObjectModel;
using System.Windows.Input;
using WindowsToSheets.Commands.Extensions;
using WindowsToSheets.Models;

namespace WindowsToSheets.ViewModels
{
	public class WindowsToSheetsViewModel : ObservableObject
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
				OnPropertyChanged();
			}
		}

		private View _viewTemplate;
		public View ViewTemplate
		{
			get => _viewTemplate;
			set
			{
				_viewTemplate = value;
				OnPropertyChanged();
			}
		}

		private int _viewScale = 20;
		public int ViewScale
		{
			get => _viewScale;
			set
			{
				_viewScale = value;
				OnPropertyChanged();
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
				OnPropertyChanged();
			}
		}

		private FamilySymbol _selectedTitleBlock;
		public FamilySymbol SelectedTitleBlock
		{
			get => _selectedTitleBlock;
			set
			{
				_selectedTitleBlock = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<FamilySymbol> TitleBlocks { get; set; }
		public ICommand CreateSheetsCommand { get; set; }

		private void LoadAllWindows()
		{
			var allWindows = _doc.GetWindows().GroupBy(x => x.GetTypeId());
			var windows = allWindows
				.Select(x => x.FirstOrDefault())
				.Where(x => x != null).ToList();

			foreach (var window in windows)
				AllWindows.Add(new WindowModel(window));
		}

		private void LoadTitleBlocks()
		{
			var titleBlocks = _doc.GetTitleBlocks();

			foreach (var tb in titleBlocks)
				TitleBlocks.Add(tb);

			_selectedTitleBlock = TitleBlocks.FirstOrDefault();
		}

		private void LoadViewTemplates()
		{
			var templates = _doc.GetViewTemplates();
			foreach (var template in templates)
				ViewTemplates.Add(template);
			_viewTemplate = ViewTemplates.FirstOrDefault();
		}

		private void SelectWindows()
		{
			SelectedWindows.Clear();
			foreach (var window in AllWindows)
				SelectedWindows.Add(window);
		}

		private void CreateViews()
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

		private void CreateSheets()
		{
			if (SelectedWindows.Count == 0 || SelectedTitleBlock == null) return;
			using (Transaction tran = new Transaction(_doc, "Creating Sheets"))
			{
				tran.Start();
				var sheet = ViewSheet.Create(_doc, SelectedTitleBlock.Id);

				if (!string.IsNullOrEmpty(SheetName))
					sheet.Name = SheetName;

				var views = SelectedWindows.Select(x => x.AssociatedView);
				sheet.PlaceViews(_doc, views);
				tran.Commit();
			}
		}
	}
}
