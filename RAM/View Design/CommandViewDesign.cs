using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAM.View_Design
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class CommandViewDesign : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;

            GridSelectionFilter selFilter = new GridSelectionFilter();

            IList<Reference> selGrid = sel.PickObjects(ObjectType.Element, selFilter, "Выберите Оси!");

            if(selGrid.Count == 0)
            {
                TaskDialog.Show("Revit","Элементы не выбраны");
                return Result.Cancelled;
            }
            List<Grid> gridList = new List<Grid>();

            foreach(Reference gridRef in selGrid)
            {
                gridList.Add(doc.GetElement(gridRef) as Grid);
            }
            if(gridList.Count == 0)
            {
                TaskDialog.Show("Revit","Оси не выбраны");
                return Result.Cancelled;

            }


            //Собираем список типов разрезов в проекте
            List<ViewFamilyType> viewFamilyType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .WhereElementIsElementType()
                .Cast<ViewFamilyType>()
                .Where(vft1 => vft1.ViewFamily == ViewFamily.Section) //|| (ViewFamily.Drafting)
                .OrderBy(vft1 => vft1.Name)
                .ToList();

            //Создаем экземпляр класса WPF
            UserViewDesign form2 = new UserViewDesign(doc, viewFamilyType);
            form2.ShowDialog();
            if(form2.DialogResult != true)
            {
                return Result.Cancelled;
            }

            ViewFamilyType selectedViewFamilyType = form2.SelectedViewFamilyType;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Построить разрез");

                BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();

                ViewSection viewSection = ViewSection.CreateSection(doc, selectedViewFamilyType.Id, sectionBox);

                t.Commit();
            }



            return Result.Succeeded;
        }
    }
}
