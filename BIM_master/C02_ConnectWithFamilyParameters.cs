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
    public class C02_ConnectWithFamilyParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FamilyManager fm = doc.FamilyManager;

            List<FamilyInstance> selection = uidoc.Selection.GetElementIds().Select(id => doc.GetElement(id)).Cast<FamilyInstance>().ToList();

            foreach (Element element in selection)
            {
                ParameterSetIterator iterator = element.Parameters.ForwardIterator();
                using (Transaction transaction = new Transaction(doc))
                {
                    transaction.Start("Связать параметры");
                    while (iterator.MoveNext())
                    {
                        Parameter parameter = (Parameter) iterator.Current;
                        if (parameter.IsShared)
                            try 
                            {
                                fm.AssociateElementParameterToFamilyParameter(parameter, fm.get_Parameter(parameter.Definition.Name));
                            }
                            catch { }
                    }
                    transaction.Commit();
                }
            }

            TaskDialog.Show("Выбраны элементы", String.Join(", ", selection.Select(element => element.Symbol.FamilyName)));
            return Result.Succeeded;
        }
    }
}
