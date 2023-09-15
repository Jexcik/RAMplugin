using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RAM.FileName
{
    public partial class FormFillNames : System.Windows.Forms.Form
    {
        public string Surname1;
        public string Surname2;
        public string Surname3;
        public string Surname4;
        public string Surname5;
        public string Surname6;

        public string surLabel1;
        public string surLabel2;
        public string surLabel3;
        public string surLabel4;
        public string surLabel5;
        public string surLabel6;

        Document Doc;

        List<FamilyInstance> TitleBlockList = new List<FamilyInstance>();
        public List<FamilyInstance> SelItem = new List<FamilyInstance>(); //Инициализируем список в который будем возвращать выбранные листы
        public List<ViewSheet> SelViewSheet = new List<ViewSheet>(); //Инициализируем список в который будет возвращать выбраные листы

        public FormFillNames(List<ViewSheet> ViewSheets, Document doc)
        {
            InitializeComponent();
            ListBox.DataSource = ViewSheets;
            ListBox.DisplayMember = "Name";
            Doc = doc;

        }

        private void FormFillNames_Load(object sender, EventArgs e)
        {
            SurLabel1.Text = surLabel1;
            SurLabel2.Text = surLabel2;
            SurLabel3.Text = surLabel3;
            SurLabel4.Text = surLabel4;
            SurLabel5.Text = surLabel5;
            SurLb6.Text = surLabel6;
        }

        private void Proverka()
        {
            bool parsed1 = int.TryParse(textBox_Surname1.Text, out int Sur1) || double.TryParse(textBox_Surname1.Text, out double Sur2);

            if (parsed1)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname1.Text}, это не правильно", "Ошибка");
                return;
            }
            bool parsed2 = int.TryParse(textBox_Surname2.Text, out int Sur3) || double.TryParse(textBox_Surname1.Text, out double Sur4);

            if (parsed2)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname2.Text}, это не правильно", "Ошибка");
                return;
            }
            bool parsed3 = int.TryParse(textBox_Surname3.Text, out int Sur5) || double.TryParse(textBox_Surname1.Text, out double Sur6);

            if (parsed3)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname3.Text}, это не правильно", "Ошибка");
                return;
            }
            bool parsed4 = int.TryParse(textBox_Surname4.Text, out int Sur7) || double.TryParse(textBox_Surname1.Text, out double Sur8);

            if (parsed4)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname4.Text}, это не правильно", "Ошибка");
                return;
            }
            bool parsed5 = int.TryParse(textBox_Surname5.Text, out int Sur9) || double.TryParse(textBox_Surname1.Text, out double Sur10);

            if (parsed5)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname5.Text}, это не правильно", "Ошибка");
                return;
            }
        }
        private void btn_ОК_Click(object sender, EventArgs e)
        {
            Proverka();
            Surname1 = textBox_Surname1.Text; //Зписываем значение в поле Фамилия 1
            Surname2 = textBox_Surname2.Text; //Зписываем значение в поле Фамилия 2
            Surname3 = textBox_Surname3.Text; //Зписываем значение в поле Фамилия 3
            Surname4 = textBox_Surname4.Text; //Зписываем значение в поле Фамилия 4
            Surname5 = textBox_Surname5.Text; //Зписываем значение в поле Фамилия 5
            Surname6 = textBox_Surname6.Text; //Зписываем значение в поле Фамилия 6

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelViewSheet = ListBox.CheckedItems.Cast<ViewSheet>().ToList();//Добавляем выбраные элементы в список и приобразуем в ViewSheet

            for (int i = 0; i < SelViewSheet.Count; i++)
            {
                //Получаем список Основных надписей размещенных на выбраных листах
                List<FamilyInstance> l1 = new FilteredElementCollector(Doc, SelViewSheet[i].Id)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsNotElementType().Cast<FamilyInstance>()
                    .ToList();

                TitleBlockList.AddRange(l1);
            }
        }

        private void checkBoxVisSign_CheckedChanged(object sender, EventArgs e)
        {
            using (Transaction t = new Transaction(Doc))
            {
                t.Start("Выключение подписи");

                foreach (FamilyInstance titleBlock in TitleBlockList)
                {
                    if (!checkBoxVisSign.Checked)
                    {
                        titleBlock.LookupParameter("Подпись_Видимость").Set(0);
                    }
                    else
                    {
                        titleBlock.LookupParameter("Подпись_Видимость").Set(1);
                    }
                }
                t.Commit();
            }
        }

        private void Check()
        {
            string CurrDate = Calendar.Value.ToString("MM.yy")??"Ошибка";
            foreach (FamilyInstance titleBlock in TitleBlockList)
            {
                if (DateCheck.Checked)
                {
                    titleBlock.LookupParameter("Строка1_Дата").Set(1);
                    titleBlock.LookupParameter("Строка2_Дата").Set(1);
                    titleBlock.LookupParameter("Строка3_Дата").Set(1);
                    titleBlock.LookupParameter("Строка4_Дата").Set(1);
                    titleBlock.LookupParameter("Строка5_Дата").Set(1);
                    titleBlock.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE).Set(CurrDate);
                }

                else
                {
                    titleBlock.LookupParameter("Строка1_Дата").Set(0);
                    titleBlock.LookupParameter("Строка2_Дата").Set(0);
                    titleBlock.LookupParameter("Строка3_Дата").Set(0);
                    titleBlock.LookupParameter("Строка4_Дата").Set(0);
                    titleBlock.LookupParameter("Строка5_Дата").Set(0);

                }
            }
        }

        private void DateCheck_CheckedChanged(object sender, EventArgs e)
        {
            using (Transaction t = new Transaction(Doc))
            {
                t.Start("Заполнение даты");
                Check();
                t.Commit();
            }

        }
    }
}
