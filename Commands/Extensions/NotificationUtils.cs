using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Binding = System.Windows.Data.Binding;
using Grid = System.Windows.Controls.Grid;

namespace WindowsToSheets.Commands.Extensions
{
	public static class NotificationUtils
	{
		public static void ShowMessageBox(this string text, string title = "Test")
		{
			MessageBox.Show(text, title);
		}

		public static void ShowMessageBox(this IEnumerable<string> text, string title = "Test")
		{
			var stringBuilder = new StringBuilder();
			text.ToList().ForEach(x => stringBuilder.AppendLine(x));
			MessageBox.Show(stringBuilder.ToString(), title);
		}

		public static void ShowMessageBox(this string[] text)
		{
			text.ToList().ShowMessageBox();
		}
		public static void ShowMessageBox(this object obj, string title = null)
		{
			//if (obj == null) throw new ArgumentException("object null");
			if (obj == null) return;

			var dataShows = new List<DataShow>();
			var type = obj.GetType();
			var propertyInfos = type.GetProperties();

			foreach (var propertyInfo in propertyInfos)
			{
				var name = propertyInfo.Name;
				var value = GetValue(propertyInfo, obj);
				var dataShow = new DataShow() { Name = name, Value = value.ToString() };
				dataShows.Add(dataShow);
			}

			var shows = dataShows.OrderBy(x => x.Name).ToList();
			var window = new Window
			{
				Title = title ?? obj.GetType().FullName,
				Width = 400,
				Height = 600,
				SizeToContent = SizeToContent.Height,
				ResizeMode = ResizeMode.NoResize,
			};

			var grid = new Grid();
			var rowDefinitions = grid.RowDefinitions;
			rowDefinitions.Add(new RowDefinition());
			var definition = new RowDefinition()
			{
				Height = new GridLength(30)
			};
			rowDefinitions.Add(definition);

			var btnClose = new Button()
			{
				IsCancel = true,
				Content = "Close",
				Height = 29,
			};
			btnClose.Click += new RoutedEventHandler((sender, args) => window.Close());

			var stackPanel = new StackPanel();
			Grid.SetRow(stackPanel, 1);
			stackPanel.Children.Add(btnClose);
			grid.Children.Add(stackPanel);

			var listView = new ListView();
			var col1 = new GridViewColumn()
			{
				Header = "Name",
				DisplayMemberBinding = new Binding("Name"),
			};
			var col2 = new GridViewColumn()
			{
				Header = "Value",
				DisplayMemberBinding = new Binding("Value"),
			};

			var gridView = new GridView();
			gridView.Columns.Add(col1);
			gridView.Columns.Add(col2);
			listView.View = gridView;
			listView.ItemsSource = shows;
			grid.Children.Add(listView);
			window.Content = grid;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window.ShowDialog();
		}

		public static object GetValue(PropertyInfo prop, object obj)
		{
			object propValue = null;
			try
			{
				propValue = prop.GetValue(obj, null);
			}
			catch
			{ }

			return propValue ?? string.Empty;
		}
	}

	public class DataShow
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}
}