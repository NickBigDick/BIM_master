using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;

namespace BIM_master
{
    public class ViewModel
    {
        public ViewModel() { }

        public string FamilyName { get; set; }

        private Family GetFamilyByName(Document doc, string name)
        {
            Family family = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(fam => fam.Name == name).FirstOrDefault();
            return family;
        }
        public void PlaceSymbols()
        {
            Document doc = RevitAPI.Document;
            Level level = doc.GetElement(doc.ActiveView.LevelId) as Level;
            StructuralType structuralType = StructuralType.UnknownFraming;
            Family family = GetFamilyByName(doc, FamilyName);
            List<FamilySymbol> familySymbols = family.GetFamilySymbolIds().Select(id => doc.GetElement(id)).Cast<FamilySymbol>().ToList();

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Расстановка типоразмеров");
                XYZ point = new XYZ();
                foreach (FamilySymbol symbol in familySymbols)
                {
                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                        doc.Regenerate();
                    }

                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, symbol, level, structuralType);
                    doc.Regenerate();
                    BoundingBoxXYZ bb = familyInstance.get_BoundingBox(null);
                    double offset = bb.Max.Y - bb.Min.Y;
                    point = new XYZ(point.X, point.Y + offset, point.Z);

                }
                transaction.Commit();
            }

            //TaskDialog.Show("!", String.Join(", ", familySymbols.Select(f => f.Name)));
        }

    }
}
