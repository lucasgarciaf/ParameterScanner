using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ParameterScanner
{
    public partial class ParameterScannerWindow : Window
    {
        private ExternalCommandData _commandData;

        public ParameterScannerWindow(ExternalCommandData commandData)
        {
            if (commandData == null)
            {
                throw new ArgumentNullException(nameof(commandData), "ExternalCommandData cannot be null.");
            }

            InitializeComponent();
            _commandData = commandData;
        }


        private void IsolateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;
            PerformSearch(isolate: true);
            //this.Close();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;
            PerformSearch(isolate: false);
            //this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ParameterNameTextBox.Text))
            {
                MessageBox.Show("Parameter name cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void PerformSearch(bool isolate)
        {
            try
            {
                if (_commandData?.Application?.ActiveUIDocument == null)
                {
                    throw new InvalidOperationException("No active Revit document found.");
                }

                // Get the user inputs
                string paramName = ParameterNameTextBox.Text.Trim();
                string paramValue = ParameterValueTextBox.Text.Trim();

                if (string.IsNullOrEmpty(paramName))
                {
                    MessageBox.Show("Parameter name cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                UIDocument uidoc = _commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                View activeView = doc.ActiveView;

                if (doc == null || activeView == null)
                {
                    throw new InvalidOperationException("No active document or view found.");
                }

                if (!IsValidView(activeView))
                {
                    StatusTextBlock.Text = "This tool only works in Floor Plans, Reflected Ceiling Plans, and 3D Views.";
                    return;
                }

                // Reset the view before collecting elements, only if isolating
                if (isolate)
                {
                    using (Transaction tx = new Transaction(doc, "Reset View"))
                    {
                        tx.Start();

                        // Deactivate all temporary view modes to reset the view
                        activeView.TemporaryViewModes.DeactivateAllModes();

                        tx.Commit();
                    }
                }

                // Now collect elements in the current view
                FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id)
                    .WhereElementIsNotElementType();

                List<Element> matchingElements = new List<Element>();


                foreach (Element element in collector)
                {
                    Parameter param = element.LookupParameter(paramName);
                    if (param == null)
                    {
                        // Skip elements that do not have the specified parameter
                        continue;
                    }

                    string value = GetParameterValue(param);
                    if (string.IsNullOrEmpty(paramValue))
                    {
                        // If the parameter value is empty, look for elements with no value
                        if (string.IsNullOrEmpty(value))
                        {
                            matchingElements.Add(element);
                        }
                    }
                    else
                    {
                        // Match elements with the specified parameter value
                        if (string.Equals(value, paramValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchingElements.Add(element);
                        }

                    }
                }

                if (matchingElements.Count == 0)
                {
                    StatusTextBlock.Text = "No elements found matching the criteria.";
                    return;
                }
                //else
                //{
                //    MessageBox.Show($"{matchingElements.Count} elements found with parameter '{paramName}' matching '{paramValue}'.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                else
                {
                    StatusTextBlock.Text = $"{matchingElements.Count} elements found matching the criteria.";
                }

                ICollection<ElementId> elementIds = matchingElements.Select(e => e.Id).ToList();

                if (isolate)
                {
                    using (Transaction tx = new Transaction(doc, "Isolate Elements"))
                    {
                        tx.Start();
                        // Isolate the matching elements
                        activeView.IsolateElementsTemporary(elementIds);
                        tx.Commit();
                    }
                }
                else
                {
                    // Clear previous selection
                    uidoc.Selection.SetElementIds(new List<ElementId>());

                    uidoc.ShowElements(elementIds);

                    uidoc.Selection.SetElementIds(elementIds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private bool IsValidView(View view)
        {
            return view is ViewPlan plan && (plan.ViewType == ViewType.FloorPlan || plan.ViewType == ViewType.CeilingPlan)
                || view is View3D;
        }

        private string GetParameterValue(Parameter param)
        {
            switch (param.StorageType)
            {
                case StorageType.String:
                    return param.AsString() ?? string.Empty;
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.Double:
                    return param.AsDouble().ToString();
                case StorageType.ElementId:
                    ElementId id = param.AsElementId();
                    return id != ElementId.InvalidElementId ? id.Value.ToString() : string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}
