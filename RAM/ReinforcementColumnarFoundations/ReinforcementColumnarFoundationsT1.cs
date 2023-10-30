using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAM.ReinforcementColumnarFoundations
{
    public class ReinforcementColumnarFoundationsT1 : IExternalCommand
    {
        public Result Execute(UIApplication uiapp, Document doc, List<FamilyInstance> foundationList, ReinforcementColumnarFoundationsWPF reinforcementColumnarFoundationsWPF)
        {
            RebarBarType firstMainBarType = reinforcementColumnarFoundationsWPF.FirstMainBarTape;
            double firstMainBarDiam = firstMainBarType.BarDiameter;
            RebarBarType firstStirrupBarTape = reinforcementColumnarFoundationsWPF.FirstStirrupBarTape;
            double firstStirrupBarDiam = firstStirrupBarTape.BarDiameter;
            RebarHookType rebarHookTypeForStirrup = reinforcementColumnarFoundationsWPF.RebarHookTypeForStirrup;

            RebarShape form01 = reinforcementColumnarFoundationsWPF.Form01;
            RebarShape form26 = reinforcementColumnarFoundationsWPF.Form26;
            RebarShape form11 = reinforcementColumnarFoundationsWPF.Form11;
            RebarShape form51 = reinforcementColumnarFoundationsWPF.Form51;

            RebarCoverType supracolumnRebarBarCoverType = reinforcementColumnarFoundationsWPF.SupracolumnRebarBarCoverType;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Армирование фундаментов - Тип 1");
                foreach (FamilyInstance foundation in foundationList)
                {
                    foundation.get_Parameter(BuiltInParameter.CLEAR_COVER_OTHER).Set(supracolumnRebarBarCoverType.Id);

                    //Точки для построения кривых стержня
                    XYZ rebar_p1 = new XYZ(Math.Round();
                    XYZ rebar_p2 = null;
                }
            }

            return Result.Succeeded;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
