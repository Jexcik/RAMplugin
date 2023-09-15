using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAM.GetElement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandGetElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            //var categories = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);

            var ProjectInfo = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ProjectInformation)
                .Cast<ProjectInfo>()
                .FirstOrDefault();

            var viewsheet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .Where(f => f.LookupParameter("Имя листа").AsString() != "Начальный вид")
                    .FirstOrDefault().LookupParameter("ADSK_Штамп_Раздел проекта").AsString();

            var ProjectNumber_Param = ProjectInfo.LookupParameter("Номер проекта").AsString();

            var AllList = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElements();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Запись параметров");
                foreach (var element in AllList)
                {
                    var category = element.Category;
                    if (category != null)
                    {
                        var builtInCategory = category.GetBuiltInCategory();
                        if (element.LookupParameter("RAM_Марка комплекта") != null && !element.LookupParameter("RAM_Марка комплекта").IsReadOnly)
                        {
                            element.LookupParameter("RAM_Марка комплекта").Set($"{ProjectNumber_Param}-{viewsheet}");

                            if (viewsheet.Contains("КМ"))
                            {
                                element.LookupParameter("RAM_Марка комплекта").Set($"{ProjectNumber_Param}-{viewsheet}");

                                var familySymbol = element as FamilySymbol;
                                if (familySymbol != null)
                                {
                                    if (familySymbol.LookupParameter("ADSK_Группа конструкций").AsValueString() == "2" && familySymbol.LookupParameter("ADSK_Тип элемента КМ").AsValueString() == "2")
                                    {
                                        element.LookupParameter("RAM_Код классификатора компонентов").Set("04_01");
                                    }
                                }
                            }
                        }
                    }
                }
                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
