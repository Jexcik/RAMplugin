using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAM.GetElement
{
    static public class CategoryExtension
    {
        public static BuiltInCategory GetBuiltInCategory(this Category category)
        {
            if (Enum.IsDefined(typeof(BuiltInCategory),
                                          category.Id.IntegerValue))
            {
                var builtInCategory = (BuiltInCategory)category.Id.IntegerValue;
                return builtInCategory;
            }

            return BuiltInCategory.INVALID;
        }
    }
}
