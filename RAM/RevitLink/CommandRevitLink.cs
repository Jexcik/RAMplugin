using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RAM.RevitLink
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandRevitLink : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            var RVTLinkList = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkType)).Cast<RevitLinkType>().ToList();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Удаление связи");

                var SharedParamElement = new FilteredElementCollector(doc)
                    .OfClass(typeof(SharedParameterElement))
                    .WhereElementIsNotElementType()
                    .Cast<SharedParameterElement>()
                    .Where(x=>x.Name.Contains("ADSK"))
                    .ToList();

                var ParameterElement= new FilteredElementCollector(doc)
                    .OfClass(typeof(ParameterElement))
                    .WhereElementIsNotElementType()
                    .Cast<ParameterElement>()
                    .Where(x => !x.Name.Contains("ADSK"))
                    .ToList();

                foreach (var element in RVTLinkList)
                {
                    doc.Delete(element.Id);
                }
                t.Commit();
            }
            //foreach (Element element in collector.OfClass(typeof(RevitLinkType)))
            //{
            //    ExternalFileReference extFileRef = element.GetExternalFileReference();
            //    if (null == extFileRef || extFileRef.GetLinkedFileStatus() != LinkedFileStatus.Loaded)
            //        continue;
            //var revitLinkType = (RevitLinkType)element;
            //    loadedExternalFilesRef.Add(revitLinkType);
            //    revitLinkType.Unload(null);
            //}
            return Result.Succeeded;
        }
    }
}
