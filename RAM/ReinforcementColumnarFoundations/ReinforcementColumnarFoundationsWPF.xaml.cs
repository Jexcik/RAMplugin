using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public string SelectedReinforcementTypeButtonName;

        RainforcementColumnarFoundationsSettings RainforcementColumnarFoundationsSettingsItem;
        RainforcementColumnarFoundationsSettingsT1 RainforcementColumnarFoundationsSettingsT1Item;
        public ReinforcementColumnarFoundationsWPF(List<RebarBarType> rebarBarTypesList, List<RebarShape> rebarShapeList, List<RebarCoverType> rebarCoverTypesList)
        {
            RebarBarTypesList = rebarBarTypesList;
            RebarShapeList = rebarShapeList;
            RebarCoverTypesList = rebarCoverTypesList;

            RainforcementColumnarFoundationsSettingsItem = new RainforcementColumnarFoundationsSettings().GetSettings();
            RainforcementColumnarFoundationsSettingsT1Item = new RainforcementColumnarFoundationsSettingsT1().GetSettings();

            InitializeComponent();

            comboBox_FirstBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_FirstBarTapes.DisplayMemberPath = "Name";

            comboBox_SecondStirrupBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_SecondStirrupBarTapes.DisplayMemberPath = "Name";

            comboBox_FirstStirrupBarTapes.ItemsSource = RebarBarTypesList;
            comboBox_FirstStirrupBarTapes.DisplayMemberPath = "Name";

            comboBox_RebarCoverTypes.ItemsSource = RebarCoverTypesList;
            comboBox_RebarCoverTypes.DisplayMemberPath = "Name";

            comboBox_Form01.ItemsSource = RebarShapeList;
            comboBox_Form01.DisplayMemberPath = "Name";

            comboBox_Form26.ItemsSource = RebarShapeList;
            comboBox_Form26.DisplayMemberPath = "Name";

            comboBox_Form11.ItemsSource = RebarShapeList;
            comboBox_Form11.DisplayMemberPath = "Name";

            comboBox_Form51.ItemsSource = RebarShapeList;
            comboBox_Form51.DisplayMemberPath = "Name";

        }

        private void buttonType1_Click(object sender, RoutedEventArgs e)
        {
            SelectedReinforcementTypeButtonName = (sender as Button).Name;

            //image_Sections.Source = new BitmapImage(new Uri("pack://application:,,, /RAM;component/Resources/Надколонник_Армирование_Тип1.png"));

            SetSavedSettingsT1();
        }

        private void SetSavedSettingsT1()
        {
            if (RainforcementColumnarFoundationsSettingsT1Item != null)
            {

            }
        }

        private void SaveSettings()
        {
            RainforcementColumnarFoundationsSettingsItem = new RainforcementColumnarFoundationsSettings();
            RainforcementColumnarFoundationsSettingsItem.SelectedTypeButtonName = SelectedReinforcementTypeButtonName;
            RainforcementColumnarFoundationsSettingsItem.SaveSettings();

            if (SelectedReinforcementTypeButtonName == "button_Type1")
            {

            }
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult= false;
            Close();
        }
    }
}
