using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIM_master
{
    [Transaction(TransactionMode.Manual)]

    public class C03_PlaceFamilySymbols : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (RevitAPI.UiApplication == null)
            {
                RevitAPI.Initialize(commandData);
            }

            var viewModel = new ViewModel();
            var view = new MainView(viewModel);
            view.ShowDialog();
            return Result.Succeeded;
        }


    }
}
