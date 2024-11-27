# ParameterScanner
Parameter Scanner is a Revit add-in that allows users to search for elements based on parameter names and values. It provides functionality to isolate matching elements in the current view or select them, enhancing efficiency in managing Revit models.

# Features
- Search elements by parameter name and value
- Isolate matching elements in the view
- Select matching elements
- Supports Floor Plans, Ceiling Plans, and 3D Views
- Simple and intuitive interface

# Installation
## Using the ZIP File
1. Download the ParameterScanner.zip from the [releases page](https://github.com/lucasgarciaf/ParameterScanner/releases/tag/v1.0.0).
2. Extract the contents.
3. Copy the ParameterScanner folder to your Revit add-ins directory: "C:\ProgramData\Autodesk\Revit\Addins\2024\"

# Usage
1. Open Revit and a supported view in the project (Floor Plan, Ceiling Plan, or 3D View).
2. Go to the Param Scanner tab and click on Parameter Scanner.
3. Enter the parameter name and optional value.
4. Click Isolate in View to isolate elements or Select to select them.
5. View the status message for results.

![](https://github.com/lucasgarciaf/ParameterScanner/blob/main/ParamScanner.gif)



## Dynamo Graphs

This repository includes also Dynamo graphs that can replace the Parameter Scanner add-in.

### Available Graphs

- **ParamScanner.dyn**: has the same functionality as the add-in.

### Usage Instructions

1. **Open Dynamo Player in Revit**:
   - Launch Revit, open a project, and start Dynamo Player.
2. **Load the Graph**:
   - In Dynamo Player, navigate to the `ParamScanner` tab to open the Window.
3. **Run the Graph**:
   - Follow any prompts or instructions within the UI.


![](https://github.com/lucasgarciaf/ParameterScanner/blob/main/DynamoGraphs/dynamoParamScanner.gif)

# License
This project is licensed under the MIT License.

Feel free to reach out if you have any questions or need assistance using these tools.

