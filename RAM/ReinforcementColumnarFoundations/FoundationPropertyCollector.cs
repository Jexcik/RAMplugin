using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public FoundationPropertyCollector(Document doc, FamilyInstance foundation)
        {

            //Базовый уровень
            BaseLevel = (doc.GetElement(foundation.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId()) as Level);
            //Отметка базового уровня
            BaseLevelElevation = Math.Round((doc.GetElement(foundation.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId()) as Level).Elevation, 6);
            //Смещение снизу
            BaseLevelOffset = Math.Round(foundation.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble(), 6);

            //Отметка уровня сверху
            TopLevelElevation = Math.Round((doc.GetElement(foundation.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId()) as Level).Elevation, 6);
            //Смещение снизу
            TopLevelOffset = Math.Round(foundation.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble(), 6);

            //Высота фундамента
            FoundationLength=
        }
    }
}
