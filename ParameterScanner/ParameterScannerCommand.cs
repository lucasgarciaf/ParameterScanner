using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ParameterScanner
{
    [Transaction(TransactionMode.Manual)]
    public class ParameterScannerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Open the WPF form
                ParameterScannerWindow window = new ParameterScannerWindow(commandData);
                window.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Log the exception message and stack trace
                TaskDialog.Show("Error", $"An error occurred: {ex.Message}\n\n{ex.StackTrace}");
                return Result.Failed;
            }
        }
    }
}
