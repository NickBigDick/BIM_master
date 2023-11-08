using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIM_master
{
    [Transaction(TransactionMode.Manual)]
    public class C01_CloseProjects : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            UIApplication uIApplication = commandData.Application;
            var documents = uIApplication.Application.Documents.ForwardIterator();
            while (documents.MoveNext())
            {
                Document current = (Document) documents.Current;
                if (current.Title != doc.Title)
                {
                    current.Close(false);
                }
            }
            
            return Result.Succeeded;
        }
    }
}
