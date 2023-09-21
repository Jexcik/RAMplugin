using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAM.FileName
{
    /// <summary>
    /// Логика взаимодействия для FillNameWPF.xaml
    /// </summary>
    public partial class FillNameWPF : Window
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
        public List<ViewSheet> SelectionViewSheet = new List<ViewSheet>();//Инициализируем список в который будет возвращать выбраные листы
        List<string> JobTitleList = new List<string> { "Разработал", "Проверил", "Тех.контр.", "Нач.отдела", "Н.контр.", "ГИП" };

        public FillNameWPF()
        {

        }
        public FillNameWPF(List<ViewSheet> ViewSheets, Document doc, FamilyInstance familyInstance)
        {

            InitializeComponent();
            ListBox_Sheets.ItemsSource = ViewSheets;
            ListBox_Sheets.DisplayMemberPath = "Name";

            Doc = doc;

            ComboBox_Surname1.Text = JobTitleList[0];
            ComboBox_Surname2.Text = JobTitleList[1];
            ComboBox_Surname3.Text = JobTitleList[2];
            ComboBox_Surname4.Text = JobTitleList[3];
            ComboBox_Surname5.Text = JobTitleList[4];
            ComboBox_Surname6.Text = JobTitleList[5];

            ComboBox_Surname1.ItemsSource = JobTitleList;
            ComboBox_Surname2.ItemsSource = JobTitleList;
            ComboBox_Surname3.ItemsSource = JobTitleList;
            ComboBox_Surname4.ItemsSource = JobTitleList;
            ComboBox_Surname5.ItemsSource = JobTitleList;
            ComboBox_Surname6.ItemsSource = JobTitleList;

            textBox_Surname1.Text = familyInstance.LookupParameter("ADSK_Штамп_1 фамилия").AsString();
            textBox_Surname2.Text = familyInstance.LookupParameter("ADSK_Штамп_2 фамилия").AsString();
            textBox_Surname3.Text = familyInstance.LookupParameter("ADSK_Штамп_3 фамилия").AsString();
            textBox_Surname4.Text = familyInstance.LookupParameter("ADSK_Штамп_4 фамилия").AsString();
            textBox_Surname5.Text = familyInstance.LookupParameter("ADSK_Штамп_5 фамилия").AsString();
            textBox_Surname6.Text = familyInstance.LookupParameter("ADSK_Штамп_6 фамилия").AsString();

            checkbox_IncludeSignature1.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 1_Видимость").AsInteger());
            checkbox_IncludeSignature2.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 2_Видимость").AsInteger());
            checkbox_IncludeSignature3.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 3_Видимость").AsInteger());
            checkbox_IncludeSignature4.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 4_Видимость").AsInteger());
            checkbox_IncludeSignature5.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 5_Видимость").AsInteger());
            checkbox_IncludeSignature6.IsChecked = Convert.ToBoolean(familyInstance.LookupParameter("Подпись 6_Видимость").AsInteger());
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            Proverka();

            DialogResult = true;
            this.Close();
        }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void checkbox_IncludeSignature_Checked(object sender, RoutedEventArgs e)
        {
            using (Transaction t = new Transaction(Doc))
            {
                t.Start("Выключение подписи");
                foreach (FamilyInstance titleBlock in TitleBlockList)
                {
                    var z = !(bool)checkbox_IncludeSignature1.IsChecked ? titleBlock.LookupParameter("Подпись 1_Видимость").Set(0) : titleBlock.LookupParameter("Подпись 1_Видимость").Set(1);

                    z = !(bool)checkbox_IncludeSignature2.IsChecked ? titleBlock.LookupParameter("Подпись 2_Видимость").Set(0) : titleBlock.LookupParameter("Подпись 2_Видимость").Set(1);

                    if (!(bool)checkbox_IncludeSignature3.IsChecked)
                    {
                        titleBlock.LookupParameter("Подпись 3_Видимость").Set(0);
                    }
                    else
                    {
                        titleBlock.LookupParameter("Подпись 3_Видимость").Set(1);
                    }
                    if (!(bool)checkbox_IncludeSignature4.IsChecked)
                    {
                        titleBlock.LookupParameter("Подпись 4_Видимость").Set(0);
                    }
                    else
                    {
                        titleBlock.LookupParameter("Подпись 4_Видимость").Set(1);
                    }
                    if (!(bool)checkbox_IncludeSignature5.IsChecked)
                    {
                        titleBlock.LookupParameter("Подпись 5_Видимость").Set(0);
                    }
                    else
                    {
                        titleBlock.LookupParameter("Подпись 5_Видимость").Set(1);
                    }
                    if (!(bool)checkbox_IncludeSignature6.IsChecked)
                    {
                        titleBlock.LookupParameter("Подпись 6_Видимость").Set(0);
                    }
                    else
                    {
                        titleBlock.LookupParameter("Подпись 6_Видимость").Set(1);
                    }
                }
                t.Commit();
            }
        }

        private void ListBox_Sheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionViewSheet = ListBox_Sheets.SelectedItems.Cast<ViewSheet>().ToList();//Добавляем выбраные элементы в список и приобразуем в ViewSheet

            for (int i = 0; i < SelectionViewSheet.Count; i++)
            {
                //Получаем список Основных надписей размещенных на выбраных листах
                List<FamilyInstance> l1 = new FilteredElementCollector(Doc, SelectionViewSheet[i].Id)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsNotElementType().Cast<FamilyInstance>()
                    .ToList();

                TitleBlockList.AddRange(l1);

            }
        }
        private void Proverka()
        {
            bool parsed1 = int.TryParse(textBox_Surname1.Text, out int Sur1) || double.TryParse(textBox_Surname1.Text, out double Sur2);

            if (parsed1)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname1.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname1 = textBox_Surname1.Text;
            }
            bool parsed2 = int.TryParse(textBox_Surname2.Text, out int Sur3) || double.TryParse(textBox_Surname2.Text, out double Sur4);

            if (parsed2)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname2.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname2 = textBox_Surname2.Text;
            }
            bool parsed3 = int.TryParse(textBox_Surname3.Text, out int Sur5) || double.TryParse(textBox_Surname3.Text, out double Sur6);

            if (parsed3)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname3.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname3 = textBox_Surname3.Text;
            }
            bool parsed4 = int.TryParse(textBox_Surname4.Text, out int Sur7) || double.TryParse(textBox_Surname4.Text, out double Sur8);

            if (parsed4)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname4.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname4 = textBox_Surname4.Text;
            }
            bool parsed5 = int.TryParse(textBox_Surname5.Text, out int Sur9) || double.TryParse(textBox_Surname5.Text, out double Sur10);

            if (parsed5)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname5.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname5 = textBox_Surname5.Text;
            }
            bool parsed6 = int.TryParse(textBox_Surname6.Text, out int Sur11) || double.TryParse(textBox_Surname6.Text, out double Sur12);

            if (parsed6)
            {
                MessageBox.Show($"Ты ввел {textBox_Surname6.Text}, это не правильно", "Ошибка");
                return;
            }
            else
            {
                Surname6 = textBox_Surname6.Text;
            }

        }
        private void Check()
        {
            var CurrDate = DataPicker_Calendar.SelectedDate.Value.ToString("MM.yy") ?? "Error404";
            foreach (FamilyInstance titleBlock in TitleBlockList)
            {
                if (ComboBox_Surname1.Text != "")
                {
                    titleBlock.LookupParameter("Строка1_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка1_Дата").Set(0);
                }
                if (ComboBox_Surname2.Text != "")
                {
                    titleBlock.LookupParameter("Строка2_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка2_Дата").Set(0);
                }
                if (ComboBox_Surname3.Text != "")
                {
                    titleBlock.LookupParameter("Строка3_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка3_Дата").Set(0);
                }
                if (ComboBox_Surname4.Text != "")
                {
                    titleBlock.LookupParameter("Строка4_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка4_Дата").Set(0);
                }
                if (ComboBox_Surname5.Text != "")
                {
                    titleBlock.LookupParameter("Строка5_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка5_Дата").Set(0);
                }
                if (ComboBox_Surname6.Text != "")
                {
                    titleBlock.LookupParameter("Строка6_Дата").Set(1);
                }
                else
                {
                    titleBlock.LookupParameter("Строка6_Дата").Set(0);
                }
                titleBlock.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE).Set(CurrDate);
            }
        }

        private void checkbox_DateCheck_Checked(object sender, RoutedEventArgs e)
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
