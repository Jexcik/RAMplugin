using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAM.ReinforcementColumnarFoundations
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class ReinforcementColumnarFoundationsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;

            //Список типов для выбора арматуры
            List<RebarBarType> rebarBarTypesList = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .OrderBy(rbt => rbt.Name)
                .ToList();

            //Список типов защитных слоев
            List<RebarCoverType> rebarCoverTypesList = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarCoverType))
                .Cast<RebarCoverType>()
                .OrderBy(rct => rct.Name)
                .ToList();

            //Формы для формы
            List<RebarShape> rebarShapeList = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarShape))
                .Cast<RebarShape>()
                .OrderBy(rs => rs.Name)
                .ToList();

            List<RebarHookType> rebarHookTypeList = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarHookType))
                .OrderBy(rht => rht.Name)
                .Cast<RebarHookType>()
                .ToList();


            ReinforcementColumnarFoundationsWPF rcfWPF = new ReinforcementColumnarFoundationsWPF(rebarBarTypesList,rebarShapeList,rebarCoverTypesList);

            rcfWPF.ShowDialog();
            if (rcfWPF.DialogResult != true)
            {
                return Result.Cancelled;
            }

            switch (rcfWPF.SelectedReinforcementTypeButtonName)
            {
                case "button_Type1":
                    ReinforcementColumnarFoundationsT1 reinforcementColumnarFoundationsT1 = new ReinforcementColumnarFoundationsT1();break;
            }

            return Result.Succeeded;
        }
    }
}
