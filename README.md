# Revit Plugin: Create Window Sheets

This Revit plugin simplifies the process of generating sheets for windows from elevation views. It automates window detection, creates views for each window, and places those views on sheets with dimensions and annotations for documentation.

## Features

- **Automatic Window Detection**: The plugin scans the model to detect all built-in faced windows.
- **Grouping by Type**: Windows are grouped by their type to streamline the process.
- **View Creation**: Elevation views are automatically generated for each window type.
- **Sheet Placement**: The views are placed on sheets, complete with dimensions and annotations for easy documentation.

## Workflow

1. **Window Detection**: The plugin identifies all windows in the active Revit document.
2. **Grouping by Type**: It groups windows based on their unique types.
3. **View Creation**: Elevation views are created for each group of windows.
4. **Sheet Placement**: The created views are placed onto sheets with automatic annotations and dimensions.

## Code Overview

The plugin's main functionality is implemented in the `CreateWindowSheets` class, which performs the following steps:

- **Retrieve Windows**: Uses `GetAllBuiltInFacedWindows()` to collect all windows in the model.
- **Group by Type**: Groups windows by type using `GroupWindowsByType()`.
- **Create Views**: Generates views for each window type with `CreateWindowView()`.
- **Place on Sheets**: Places the views onto sheets with `PlaceViewsOnSheets()`.

### Code Snippet

```csharp
[Transaction(TransactionMode.Manual)]
public class CreateWindowSheets : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIApplication uiapp = commandData.Application;
        Document doc = uiapp.ActiveUIDocument.Document;

        try
        {
            List<FamilyInstance> windows = doc.GetAllBuiltInFacedWindows();
            var groupedWindows = windows.GroupWindowsByType();
            List<View> views = new List<View>();

            foreach (var windowGroup in groupedWindows)
            {
                View view = doc.CreateWindowView(windowGroup.Value.FirstOrDefault());
                views.Add(view);
            }

            doc.PlaceViewsOnSheets(views);

            return Result.Succeeded;
        }
        catch (Exception e)
        {
            message = e.Message;
            return Result.Failed;
        }
    }
}
```

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   ```

2. Open the solution in Visual Studio.

3. Build the project and copy the generated `.dll` file into your Revit add-ins folder.

## Usage

1. Open Revit and load your project.
2. Run the `Create Window Sheets` command from the plugin.
3. The plugin will automatically generate window views, dimension them, and place them on sheets for documentation.

## Example Output

Here�s an example of What I am trying to do. the generated window sheet with views and annotations:

![Window Sheets Example](./Resources/window_sheet_example.png)
![Window Sheets Example](./Resources/WindowsToSheetUI.png)

## Contributing

Contributions are welcome! If you encounter any issues or want to improve this project, feel free to submit an issue or a pull request.
