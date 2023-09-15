using System;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Structure;

namespace RAM.View_Design
{
    internal class GridSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if(elem is Grid || elem is Wall )
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
