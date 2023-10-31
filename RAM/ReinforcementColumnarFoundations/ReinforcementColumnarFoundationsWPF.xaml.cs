using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAM.ReinforcementColumnarFoundations
{
    /// <summary>
    /// Логика взаимодействия для ReinforcementColumnarFoundationsWPF.xaml
    /// </summary>
    public partial class ReinforcementColumnarFoundationsWPF : Window
    {
        List<RebarBarType> RebarBarTypesList;
        List<RebarCoverType> RebarCoverTypesList;
        List<RebarShape> RebarShapeList;
        List<RebarHookType> RebarHookTypeList;

        public string SelectedReinforcementTypeButtonName;
        public RebarBarType FirstMainBarTape;
        public RebarBarType SecondMainBarTape;
        public RebarBarType FirstStirrupBarTape;
        public RebarBarType SecondStirrupBarTape;
        public RebarCoverType SupracolumnRebarBarCoverType;
        public RebarCoverType BottomRebarCoverType;

        public RebarShape Form01;
        public RebarShape Form26;
        public RebarShape Form11;
        public RebarShape Form51;
        public RebarHookType RebarHookTypeForStirrup;


        RainforcementColumnarFoundationsSettings ReinforcementColumnarFoundationsSettingsItem;
        RainforcementColumnarFoundationsSettingsT1 ReinforcementColumnarFoundationsSettingsT1Item;

        public ReinforcementColumnarFoundationsWPF(List<RebarBarType> rebarBarTypesList, List<RebarShape> rebarShapeList, List<RebarCoverType> rebarCoverTypesList, List<RebarHookType> rebarHookTypeList)
        {
            RebarBarTypesList = rebarBarTypesList;
            RebarShapeList = rebarShapeList;
            RebarCoverTypesList = rebarCoverTypesList;
            RebarHookTypeList = rebarHookTypeList;

            ReinforcementColumnarFoundationsSettingsItem = new RainforcementColumnarFoundationsSettings().GetSettings();
            ReinforcementColumnarFoundationsSettingsT1Item = new RainforcementColumnarFoundationsSettingsT1().GetSettings();

            InitializeComponent();

            comboBox_FirstBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_FirstBarTapes.DisplayMemberPath = "Name";

            comboBox_SecondStirrupBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_SecondStirrupBarTapes.DisplayMemberPath = "Name";

            comboBox_FirstStirrupBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_FirstStirrupBarTapes.DisplayMemberPath = "Name";

            comboBox_RebarCoverTypes.ItemsSource = RebarCoverTypesList;
            comboBox_RebarCoverTypes.DisplayMemberPath = "Name";

            comboBox_RebarCoverBottom.ItemsSource = RebarCoverTypesList;
            comboBox_RebarCoverBottom.DisplayMemberPath = "Name";

            comboBox_Form01.ItemsSource = RebarShapeList;
            comboBox_Form01.DisplayMemberPath = "Name";

            comboBox_Form26.ItemsSource = RebarShapeList;
            comboBox_Form26.DisplayMemberPath = "Name";

            comboBox_Form11.ItemsSource = RebarShapeList;
            comboBox_Form11.DisplayMemberPath = "Name";

            comboBox_Form51.ItemsSource = RebarShapeList;
            comboBox_Form51.DisplayMemberPath = "Name";

            comboBox_RebarHookType.ItemsSource = RebarHookTypeList;
            comboBox_RebarHookType.DisplayMemberPath = "Name";

            if (ReinforcementColumnarFoundationsSettingsItem != null)
            {
                switch (ReinforcementColumnarFoundationsSettingsItem.SelectedTypeButtonName)
                {
                    case "button_Type1": buttonType1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent)); break;
                }
            }
            else
            {
                buttonType1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }

        }

        private void buttonType1_Click(object sender, RoutedEventArgs e)
        {
            SelectedReinforcementTypeButtonName = (sender as Button).Name;
            SetBorderForSelectedButton(sender);
            SetBorderForNonSelectedButtons(sender);

            //Типы арматуры в сечении
            comboBox_FirstBarTapes.Visibility = System.Windows.Visibility.Visible;
            comboBox_FirstBarTapes.Margin = new Thickness(30, 34, 0, 0);


            SetSavedSettingsT1();
        }

        private void SetSavedSettingsT1()
        {
            if (ReinforcementColumnarFoundationsSettingsT1Item != null)
            {
                //Задание сохраненных форм
                if (RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form01Name) != null)
                {
                    comboBox_Form01.SelectedItem = RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form01Name);
                }
                else
                {
                    if (comboBox_Form01.Items.Count != 0)
                    {
                        comboBox_Form01.SelectedItem = comboBox_Form01.Items.GetItemAt(0);
                    }
                }
                if (RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form26Name) != null)
                {
                    comboBox_Form26.SelectedItem = RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form26Name);
                }
                else
                {
                    if (comboBox_Form26.Items.Count != 0)
                    {
                        comboBox_Form26.SelectedItem = comboBox_Form26.Items.GetItemAt(0);
                    }
                }

                if (RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form11Name) != null)
                {
                    comboBox_Form11.SelectedItem = RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form11Name);
                }
                else
                {
                    if (comboBox_Form11.Items.Count != 0)
                    {
                        comboBox_Form11.SelectedItem = comboBox_Form11.Items.GetItemAt(0);
                    }
                }

                if (RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form51Name) != null)
                {
                    comboBox_Form51.SelectedItem = RebarShapeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.Form51Name);
                }
                else
                {
                    if (comboBox_Form51.Items.Count != 0)
                    {
                        comboBox_Form51.SelectedItem = comboBox_Form51.Items.GetItemAt(0);
                    }
                }

                if (RebarHookTypeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.RebarHookTypeForStirrupName) != null)
                {
                    comboBox_RebarHookType.SelectedItem = RebarHookTypeList.FirstOrDefault(rbt => rbt.Name == ReinforcementColumnarFoundationsSettingsT1Item.RebarHookTypeForStirrupName);
                }
                else
                {
                    if (comboBox_RebarHookType.Items.Count != 0)
                    {
                        comboBox_RebarHookType.SelectedItem = comboBox_RebarHookType.Items.GetItemAt(0);
                    }
                }



            }
        }

        private void SaveSettings()
        {
            ReinforcementColumnarFoundationsSettingsItem = new RainforcementColumnarFoundationsSettings();
            ReinforcementColumnarFoundationsSettingsItem.SelectedTypeButtonName = SelectedReinforcementTypeButtonName;
            ReinforcementColumnarFoundationsSettingsItem.SaveSettings();

            //Проверка выбора форм стержней
            Form01 = comboBox_Form01.SelectedItem as RebarShape;
            if (Form01 == null)
            {
                TaskDialog.Show("Revit", "Выберите форму арматуры для прямых стержней (Форма 01) чтобы продолжить работу!");
                return;
            }
            Form26 = comboBox_Form26.SelectedItem as RebarShape;
            if (Form26 == null)
            {
                TaskDialog.Show("Revit", "Выберите форму арматуры для Z-образных стержней (Форма 26), чтобы продолжить работу!");
                return;
            }
            Form11 = comboBox_Form11.SelectedItem as RebarShape;
            if (Form11 == null)
            {
                TaskDialog.Show("Revit", "Выберите форму арматуры для Г-образных стержней (Форма 11), чтобы продолжить работу!");
                return;
            }
            Form51 = comboBox_Form51.SelectedItem as RebarShape;
            if (Form51 == null)
            {
                TaskDialog.Show("Revit", "Выберите форму арматуры для хомутов (Форма 51, 52 и т.д.), чтобы продолжить работу!");
                return;
            }
            RebarHookTypeForStirrup = comboBox_RebarHookType.SelectedItem as RebarHookType;
            if (RebarHookTypeForStirrup == null)
            {
                TaskDialog.Show("Revit", "Выберите тип отгибов для хомута, что бы продолжить работу!");
                return;
            }

            //Проверка заполнения полей в сечении для всех типов
            FirstMainBarTape = comboBox_FirstBarTapes.SelectedItem as RebarBarType;
            if (FirstMainBarTape == null)
            {
                TaskDialog.Show("Revit", "Выберите тип основных стержней подколонника !");
                return;
            }
            FirstStirrupBarTape = comboBox_FirstStirrupBarTapes.SelectedItem as RebarBarType;
            if (FirstStirrupBarTape == null)
            {
                TaskDialog.Show("Revit", "Выберите тип стержня основного хомута, что бы продолжить работу!");
                return;
            }
            SupracolumnRebarBarCoverType = comboBox_RebarCoverTypes.SelectedItem as RebarCoverType;
            if (SupracolumnRebarBarCoverType == null)
            {
                TaskDialog.Show("Revit", "Укажите защитный слой, что бы продолжить работу!");
                return;
            }
            BottomRebarCoverType = comboBox_RebarCoverBottom.SelectedItem as RebarCoverType;
            if (BottomRebarCoverType == null)
            {
                TaskDialog.Show("Revit", "Укажите защитный слой арматуры в подошве фундамента");
                return;
            }


            //Сохранение настроек
            if (SelectedReinforcementTypeButtonName == "button_Type1")
            {
                ReinforcementColumnarFoundationsSettingsT1Item = new RainforcementColumnarFoundationsSettingsT1();

                ReinforcementColumnarFoundationsSettingsT1Item.Form01Name = Form01.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.Form26Name = Form26.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.Form11Name = Form11.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.Form51Name = Form51.Name;

                ReinforcementColumnarFoundationsSettingsT1Item.FirstMainBarTapeName = FirstMainBarTape.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.FirstStirrupBarTapeName = FirstStirrupBarTape.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.SupracolumnRebarBarCoverTypeName = SupracolumnRebarBarCoverType.Name;
                ReinforcementColumnarFoundationsSettingsT1Item.BottomRebarCoverTypeName = BottomRebarCoverType.Name;
            }
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            DialogResult = true;
            Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private static void SetBorderForSelectedButton(object sender)
        {
            BrushConverter bc = new BrushConverter();
            (sender as Button).BorderThickness = new Thickness(4, 4, 4, 4);
        }
        private void SetBorderForNonSelectedButtons(object sender)
        {
            BrushConverter bc = new BrushConverter();
            IEnumerable<Button> buttonsSet = buttonsTypeGrid.Children.OfType<Button>()
                .Where(b => b.Name.StartsWith("button_Type"))
                .Where(b => b.Name != (sender as Button).Name);
            foreach (Button btn in buttonsSet)
            {
                btn.BorderThickness = new Thickness(1, 1, 1, 1);
            }
        }
    }
}
