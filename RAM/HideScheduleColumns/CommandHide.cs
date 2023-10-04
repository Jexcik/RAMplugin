using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RAM.HideScheduleColumns
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandHide : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View view = doc.ActiveView;

            var ViewScheduleList = new FilteredElementCollector(doc, view.Id)
                .OfCategory(BuiltInCategory.OST_ScheduleGraphics)
                .Cast<ScheduleSheetInstance>()
                .Where(x => x.Name.Contains("ВРС") || x.Name.Contains("Ведомость расхода стали"))
                .Select(x => doc.GetElement(x.ScheduleId) as ViewSchedule)
                .ToList();

            var SchedDefinitionList = ViewScheduleList.Select(x => x.Definition).ToList();


            using (Transaction t = new Transaction(doc))
            {
                t.Start("Подчистить ВРС");

                foreach (var viewSchedule in ViewScheduleList)
                {
                   ScheduleSortGroupField SortField= viewSchedule.Definition.GetSortGroupField(0);//Получаем значение первого поля в сортировке спецификации

                    viewSchedule.Definition.ClearSortGroupFields();//Очищаем поля сортировки и группировки в спецификации

                    var CountColumn = viewSchedule.GetTableData().GetSectionData(SectionType.Body).NumberOfColumns;//Получаем колличество видимых столбцов
                    var CountRow = viewSchedule.GetTableData().GetSectionData(SectionType.Body).NumberOfRows;//Получаем колличество видимых строк
                    var count = viewSchedule.Definition.GetFieldCount();//Получаем общее число столбцов в спецификации
                    List<int> columnViewList = new List<int>(); //Заводим список для индексов видимых столбцов
                    for (int i = 0; i < count; i++) //Проходимся по всем столбцам в спецификации
                    {
                        string columnName = viewSchedule.Definition.GetField(i).GetName();//Получаем имя столбца
                        if ((columnName.Contains("Ø") && !columnName.Contains("Ø_arm")) 
                            || columnName.Contains("Итого")
                            ||columnName.Contains("⌶")
                            ||columnName.Contains("L")
                            ||columnName.Contains("")
                            ||columnName.Contains("ГОСТ 103-76")
                            ||columnName.Contains("ГОСТ 30245-2003")) //Если имя столбца содержит "Ø" или Итого
                        {
                            viewSchedule.Definition.GetField(i).IsHidden = false; //Переключаем флажок видимости на выкл
                        }
                        bool condition = viewSchedule.Definition.GetField(i).IsHidden;//Проверяем видимость всех столбцев
                        if (condition == false) //Если столбец видим
                        {
                            int index = viewSchedule.Definition.GetField(i).FieldIndex; //Записываем в переменную индекс столбца
                            columnViewList.Add(index); //Добавляем индекс столбца в список индексов
                        }
                    }
                    CountColumn = viewSchedule.GetTableData().GetSectionData(SectionType.Body).NumberOfColumns; // Обновляем кол-во видимых столбцев 
                    for (int i = 0; i < CountColumn; i++)// Проходимся по всем видимым столбцам спецификации
                    {
                        int index = columnViewList[i]; //Получаем индекс видимого столбца 
                        var m = viewSchedule.GetCellText(SectionType.Body, 7, i); //Берем значение в каждой ячейки видимого столбца 
                        if (m == "0") //Если значение == 0
                        {
                            viewSchedule.Definition.GetField(index).IsHidden = true; //То выключаем видимость столбца
                        }
                    }
                    
                    viewSchedule.Definition.InsertSortGroupField(SortField,0);//Вставляем в первое поле сортировки нужное поле
                }
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
