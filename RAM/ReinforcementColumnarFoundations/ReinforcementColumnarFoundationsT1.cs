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

            RebarCoverType scRebarBarCoverType = reinforcementColumnarFoundationsWPF.SupracolumnRebarBarCoverType;
            double coverDistance = scRebarBarCoverType.CoverDistance;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Армирование фундаментов - Тип 1");
                foreach (FamilyInstance foundation in foundationList)
                {
                    FoundationPropertyCollector foundationProperty = new FoundationPropertyCollector(doc, foundation);
                    foundation.get_Parameter(BuiltInParameter.CLEAR_COVER_OTHER).Set(scRebarBarCoverType.Id);
                    var coverTop=foundation.get_Parameter(BuiltInParameter.CLEAR_COVER_TOP).AsValueString();

                    //Точки для построения кривых стержня
                    XYZ rebar_p1 = new XYZ(Math.Round(foundationProperty.FoundationBasePoint.X - foundationProperty.ColumnLength / 2 + firstMainBarDiam / 2 + coverDistance, 6), Math.Round(foundationProperty.FoundationBasePoint.Y + foundationProperty.ColumnWidth / 2 - firstMainBarDiam / 2 - coverDistance, 6), Math.Round(foundationProperty.FoundationBasePoint.Z + foundationProperty.FoundationLength, 6));
                    XYZ rebar_p2 = new XYZ(Math.Round(rebar_p1.X, 6), Math.Round(rebar_p1.Y, 6), Math.Round(rebar_p1.Z - foundationProperty.FoundationLength, 6));
                    XYZ rebar_p3 = new XYZ(Math.Round(rebar_p2.X - 300 / 308.4, 6), Math.Round(rebar_p2.Y, 6), Math.Round(rebar_p2.Z, 6));

                    //Кривые стержня
                    List<Curve> mainRebarCurves = new List<Curve>();
                    Curve line1 = Line.CreateBound(rebar_p1, rebar_p2) as Curve;
                    mainRebarCurves.Add(line1);
                    Curve line2 = Line.CreateBound(rebar_p2, rebar_p3) as Curve;
                    mainRebarCurves.Add(line2);

                    //Создание арматурного стержня
                    

                    Rebar MainRebar_1 = null;
                    try
                    {
                        MainRebar_1 = Rebar.CreateFromCurvesAndShape(doc
                            , form11
                            , firstMainBarType
                            , null
                            , null
                            , foundation
                            , new XYZ(0, 1, 0)
                            , mainRebarCurves
                            , RebarHookOrientation.Right
                            , RebarHookOrientation.Right);
                    }
                    catch
                    {
                        TaskDialog.Show("Revit", "Не удалость создать Г-образный стержень");
                        return Result.Cancelled;
                    }

                }
                t.Commit();
            }

            return Result.Succeeded;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
