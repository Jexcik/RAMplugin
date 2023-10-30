using Aspose.Cells.Charts;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RAM.ReinforcementColumnarFoundations
{
    class FoundationPropertyCollector
    {
        public Level BaseLevel { get; }
        public double BaseLevelElevation { get; }
        public double BaseLevelOffset { get; }
        public double TopLevelElevation { get; }
        public double TopLevelOffset { get; }
        public double FoundationLength { get; }
        public double ColumnHeight { get; }
        public XYZ FoundationBasePoint { get; }

        public FoundationPropertyCollector(Document doc, FamilyInstance foundation)
        {
            FamilySymbol familySymbol = foundation.Symbol;
            //Базовый уровень
            BaseLevel = (doc.GetElement(foundation.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM).AsElementId()) as Level);
            //Отметка базового уровня
            BaseLevelElevation = Math.Round((doc.GetElement(foundation.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM).AsElementId()) as Level).Elevation, 6);
            //Смещение от уровня
            BaseLevelOffset = Math.Round(foundation.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble(), 6);

            //Высота надколонника
            ColumnHeight = familySymbol.LookupParameter("Подколонник_Высота").AsDouble();
            double columnWidth = familySymbol.LookupParameter("Подколонник_Ширина").AsDouble();
            double columnLength = familySymbol.LookupParameter("Подколонник_Длина").AsDouble();


            //Высота первого уступа
            double ledge1Height = familySymbol.LookupParameter("Уступ 1_Высота").AsDouble();
            //Высота второго уступа
            double ledge2Height = familySymbol.LookupParameter("Уступ 2_Высота").AsDouble();
            //Высота третьего уступа
            double ledge3Height = familySymbol.LookupParameter("Уступ 3_Высота").AsDouble();


            //Колличество уступов
            int numberofLedges = familySymbol.LookupParameter("Плита_Количество уступов").AsInteger();


            //Высота фундамента
            FoundationLength = 0;

            switch (numberofLedges)
            {
                case 1: FoundationLength = ColumnHeight + ledge1Height; break;
                case 2: FoundationLength = ColumnHeight + ledge1Height + ledge2Height; break;
                case 3: FoundationLength = ColumnHeight + ledge1Height + ledge2Height + ledge3Height; break;
            }
            //Нижняя точка геометрии фундамента
            FoundationBasePoint = (foundation.Location as LocationPoint).Point;
        }
    }
}
