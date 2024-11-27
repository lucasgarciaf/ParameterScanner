using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Reflection;
using System.IO;



namespace ParameterScanner
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            string tabName = "Param Scanner";
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception)
            {
                // Tab already exists, do nothing
            }

            // Create a ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Tools");

            // Create a push button to launch the add-in
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData(
                "ParameterScannerButton",
                "Parameter Scanner",
                assemblyPath,
                "ParameterScanner.ParameterScannerCommand"); // Full class name of the command

            // Assign icons to the button
            string iconFolder = Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources");

            // Large Icon (32x32 pixels)
            string largeIconPath = Path.Combine(iconFolder, "icons8-parameter-32.png");
            Uri largeIconUri = new Uri(largeIconPath, UriKind.Absolute);
            BitmapImage largeImage = new BitmapImage(largeIconUri);
            buttonData.LargeImage = largeImage;

            // Small Icon (16x16 pixels)
            string smallIconPath = Path.Combine(iconFolder, "icons8-parameter-16.png");
            Uri smallIconUri = new Uri(smallIconPath, UriKind.Absolute);
            BitmapImage smallImage = new BitmapImage(smallIconUri);
            buttonData.Image = smallImage;

            // Add the button to the ribbon panel
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Clean up when Revit shuts down
            return Result.Succeeded;
        }
    }
}