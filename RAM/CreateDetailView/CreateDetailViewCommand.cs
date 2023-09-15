using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RAM.GetElement;
using RAM.View_Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RAM.CreateDetailView
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateDetailViewCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection selection = commandData.Application.ActiveUIDocument.Selection;

            List<ViewSheet> viewSheetList = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .WhereElementIsNotElementType()
                .Cast<ViewSheet>()
                .ToList();

            //Вызываем форму
            CreateDetailViewWPF createDetailViewWPF = new CreateDetailViewWPF(doc, viewSheetList);
            createDetailViewWPF.ShowDialog();
            if (createDetailViewWPF.DialogResult != true)
            {
                return Result.Cancelled;
            }

            ViewFamilyType selectedViewFamilyType = createDetailViewWPF.SelectedViewFamilyType;
            string selectedBuildByName = createDetailViewWPF.SelectedBuildByName;
            string selectedUseToBuildName = createDetailViewWPF.SelectedUseToBuildName;
            bool useTamplate = createDetailViewWPF.UseTemplate;
            ViewSection viewSectionTemplate = createDetailViewWPF.ViewSectionTemplate;
            ViewSheet selectedViewSheet = createDetailViewWPF.SelectedViewSheet;

            double indent = createDetailViewWPF.Indent;
            double projectionDepth = createDetailViewWPF.ProjectionDepth;

            string SectionName = createDetailViewWPF.SectionName;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Создание вида узла");
                GroupSelectionFilter groupSelectionFilter = new GroupSelectionFilter();
                StructConnectionsSelectionFilter structConnectionsSelectionFilter = new StructConnectionsSelectionFilter();
                XYZ pickedPoint = null;
                List<FamilyInstance> FamilyInstanceList = new List<FamilyInstance>();
                List<Group> groupsList = new List<Group>();
                IList<Reference> referenceList = null;

                try
                {
                    referenceList = selection.PickObjects(ObjectType.Element, groupSelectionFilter, "Выберите узел!");

                    foreach (Reference reference in referenceList)
                    {
                        Group group = doc.GetElement(reference) as Group;
                        groupsList.Add(group);
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    return Result.Cancelled;
                }

                foreach (Group group in groupsList)
                {
                    if (group != null)
                    {

                        List<ViewSection> viewSectionsList = new List<ViewSection>();
                        if (selectedUseToBuildName == "rbt_Section")
                        {
                            //XYZ familyOrientation = null;
                            //if (familyInstance.FacingFlipped)
                            //{
                            //    familyOrientation = familyInstance.FacingOrientation.Negate();
                            //}
                            //else
                            //{
                            //    familyOrientation = familyInstance.FacingOrientation;
                            //}
                            //curve = (familyInstance.Location as LocationCurve).Curve;

                            //if (curve is Line)
                            //{
                            //XYZ start = curve.GetEndPoint(0);
                            //XYZ end = curve.GetEndPoint(1);

                            //XYZ pointH = curve.Project(new XYZ(pickedPoint.X, pickedPoint.Y, start.Z)).XYZPoint;
                            //XYZ normalVector = (new XYZ(pickedPoint.X, pickedPoint.Y, start.Z) - pointH).Normalize();

                            //if (normalVector.IsAlmostEqualTo(familyOrientation))
                            //{
                            //    start = curve.GetEndPoint(0);
                            //    end = curve.GetEndPoint(1);
                            //}
                            //else
                            //{
                            //    start = curve.GetEndPoint(1);
                            //    end = curve.GetEndPoint(0);
                            //}

                            //XYZ curveDir = (end - start).Normalize();
                            //double w = (end - start).GetLength();

                            //Transform curveTransform = curve.ComputeDerivatives(0.5, true);
                            //XYZ origin = curveTransform.Origin;
                            //XYZ right = curveDir;
                            //XYZ up = XYZ.BasisZ;
                            //XYZ viewdir = curveDir.CrossProduct(up).Normalize();

                            BoundingBoxXYZ groupBb = group.get_BoundingBox(null);
                            double minX = groupBb.Min.X;
                            double maxX = groupBb.Max.X;

                            double minY = groupBb.Min.Y;
                            double maxY = groupBb.Max.Y;

                            double minZ = groupBb.Min.Z;
                            double maxZ = groupBb.Max.Z;

                            Transform transform = Transform.Identity;
                            //transform.Origin = origin;
                            //transform.BasisX = right;
                            //transform.BasisY = up;
                            //transform.BasisZ = viewdir;

                            BoundingBoxXYZ sectionBox = group.get_BoundingBox(null);
                            sectionBox.Transform = transform;
                            sectionBox.Min = new XYZ(minX, minY, -indent + minZ);
                            sectionBox.Max = new XYZ(maxX, maxY, projectionDepth);

                            ViewSection viewSection = ViewSection.CreateSection(doc, selectedViewFamilyType.Id, sectionBox);
                            viewSection.Name = $"{SectionName}_{viewSection.Id}";

                            if (useTamplate)
                            {
                                viewSection.get_Parameter(BuiltInParameter.VIEW_TEMPLATE).Set(viewSectionTemplate.Id);
                            }
                            viewSectionsList.Add(viewSection);

                            if (selectedViewSheet != null)
                            {
                                List<FamilyInstance> titleBlockList = new FilteredElementCollector(doc, selectedViewSheet.Id)
                                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                    .Cast<FamilyInstance>()
                                    .ToList();
                                XYZ insertPoint = new XYZ(0, 0, 0);
                                if (titleBlockList.Count != 0)
                                {
                                    FamilyInstance titleBlock = titleBlockList.First();
                                    BoundingBoxXYZ bb = titleBlock.get_BoundingBox(selectedViewSheet);

                                    double MinX = bb.Min.X;
                                    double MaxY = bb.Max.Y;

                                    insertPoint = new XYZ(MinX + 30 / 304.8, MaxY - 20 / 304.8, 0);
                                }
                                viewSectionsList.Reverse();
                                double maxHight = 0;
                                foreach (ViewSection vSection in viewSectionsList)
                                {
                                    Viewport viewport = Viewport.Create(doc, selectedViewSheet.Id, vSection.Id, insertPoint);

                                    int viewScale = viewSection.Scale;
                                    BoundingBoxXYZ cropbox = viewSection.CropBox;
                                    XYZ P1 = new XYZ(cropbox.Max.X / viewScale, cropbox.Max.Y / viewScale, 0);
                                    XYZ P2 = new XYZ(cropbox.Min.X / viewScale, cropbox.Min.Y / viewScale, 0);

                                    double deltaX = new XYZ(P1.X, 0, 0).DistanceTo(new XYZ(P2.X, 0, 0)) / 2;
                                    double deltaY = new XYZ(0, P1.Y, 0).DistanceTo(new XYZ(0, P2.Y, 0)) / 2;
                                    ElementTransformUtils.MoveElement(doc, viewport.Id, new XYZ(deltaX, -deltaY, 0));

                                    insertPoint = new XYZ(insertPoint.X + deltaX * 2, insertPoint.Y, insertPoint.Z);
                                    if (deltaY * 2 > maxHight)
                                    {
                                        maxHight = deltaY * 2;
                                    }
                                }
                            }
                            //}
                        }
                    }
                }

                t.Commit();
            }

            return Result.Succeeded;
        }
        private static List<FamilyInstance> GetFamilyInstanceFromCurrentSelection(Document doc, Selection selection)
        {
            ICollection<ElementId> familyIds = selection.GetElementIds();
            List<FamilyInstance> tempFMiList = new List<FamilyInstance>();
            foreach (ElementId familyId in familyIds)
            {
                if (doc.GetElement(familyId) is FamilyInstance && doc.GetElement(familyId).Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_StructConnections))
                {
                    tempFMiList.Add(doc.GetElement(familyId) as FamilyInstance);
                }
            }
            return tempFMiList;
        }
    }
}
