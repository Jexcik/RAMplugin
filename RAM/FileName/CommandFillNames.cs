using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RAM.FileName;

namespace RAM.FileName
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class CommandFillNames : IExternalCommand
    {
        List<FamilyInstance> TitleBlockList = new List<FamilyInstance>(); //список выбранных листов основной надписи

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document; //Получаем открытый проект
            View view = doc.ActiveView;

            List<string> ListSheets = new List<string>();//Инициализируем список для названия листов

            FamilyInstance familyInstance = new FilteredElementCollector(doc, view.Id)
               .OfCategory(BuiltInCategory.OST_TitleBlocks)
               .WhereElementIsNotElementType()
               .Cast<FamilyInstance>()
               .FirstOrDefault(x => x.LookupParameter("ADSK_Штамп_1 фамилия") != null)
               ?? new FilteredElementCollector(doc)
               .OfCategory(BuiltInCategory.OST_TitleBlocks)
               .WhereElementIsNotElementType().Cast<FamilyInstance>()
               .FirstOrDefault(x => x.LookupParameter("ADSK_Штамп_1 фамилия") != null);

            //Собираем список листов в проекте
            List<ViewSheet> ViewSheetsList = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .OrderBy(vs => vs.SheetNumber)
                    .Where(f => f.LookupParameter("Имя листа").AsString() != "Начальный вид")
                    .ToList();

            //Объявляем класс формы
            FillNameWPF fillNameWPF = new FillNameWPF(ViewSheetsList, doc, familyInstance);


            fillNameWPF.ShowDialog();

            if (fillNameWPF.DialogResult != true)
            {
                return Result.Cancelled;
            }

            for (int i = 0; i < fillNameWPF.SelectionViewSheet.Count; i++)
            {
                //Получаем список Основных надписей размещенных на выбраных листах
                List<FamilyInstance> l1 = new FilteredElementCollector(doc, fillNameWPF.SelectionViewSheet[i].Id)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsNotElementType().Cast<FamilyInstance>()
                    .ToList();

                TitleBlockList.AddRange(l1);
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Заполнение штампа");

                foreach (ViewSheet viewSheet in ViewSheetsList)
                {
                    viewSheet.LookupParameter("ADSK_Штамп_1 должность").Set(fillNameWPF.ComboBox_Surname1.Text);
                    viewSheet.LookupParameter("ADSK_Штамп_2 должность").Set(fillNameWPF.ComboBox_Surname2.Text);
                    viewSheet.LookupParameter("ADSK_Штамп_3 должность").Set(fillNameWPF.ComboBox_Surname3.Text);
                    viewSheet.LookupParameter("ADSK_Штамп_4 должность").Set(fillNameWPF.ComboBox_Surname4.Text);
                    viewSheet.LookupParameter("ADSK_Штамп_5 должность").Set(fillNameWPF.ComboBox_Surname5.Text);
                    viewSheet.LookupParameter("ADSK_Штамп_6 должность").Set(fillNameWPF.ComboBox_Surname6.Text);
                }

                //Проходимся по каждому листу и в параметр ADSK_Штамп_1 фамилия записываем значение из формы
                foreach (FamilyInstance titleBlock in TitleBlockList)
                {
                    if (titleBlock.LookupParameter("ADSK_Штамп_1 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_1 фамилия").Set(fillNameWPF.Surname1);
                    }
                    if (titleBlock.LookupParameter("ADSK_Штамп_2 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_2 фамилия").Set(fillNameWPF.Surname2);
                    }
                    if (titleBlock.LookupParameter("ADSK_Штамп_3 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_3 фамилия").Set(fillNameWPF.Surname3);
                    }
                    if (titleBlock.LookupParameter("ADSK_Штамп_4 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_4 фамилия").Set(fillNameWPF.Surname4);
                    }
                    if (titleBlock.LookupParameter("ADSK_Штамп_5 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_5 фамилия").Set(fillNameWPF.Surname5);
                    }
                    if (titleBlock.LookupParameter("ADSK_Штамп_6 фамилия") != null) //Проверяем каждое семейство  штампа на наличие параметра
                    {
                        titleBlock.LookupParameter("ADSK_Штамп_6 фамилия").Set(fillNameWPF.Surname6);
                    }
                }
                t.Commit();
            }

            return Result.Succeeded;


        }
    }
}
