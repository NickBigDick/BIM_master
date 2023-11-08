using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BIM_master
{
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";
            string tabName = "BIM-Master";

            application.CreateRibbonTab(tabName);
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Вспомогательные");
                panel.AddItem(new PushButtonData(nameof(C01_CloseProjects), "Закрыть документы", assemblyLocation, typeof(C01_CloseProjects).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "Close.png"))
                });

                panel.AddItem(new PushButtonData(nameof(C02_ConnectWithFamilyParameters), "Связать параметры", assemblyLocation, typeof(C02_ConnectWithFamilyParameters).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "Connect_parameters.png"))
                });
            }
            return Result.Succeeded;
        }
    }
}
