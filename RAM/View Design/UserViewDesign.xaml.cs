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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RAM.View_Design
{
    /// <summary>
    /// Логика взаимодействия для UserViewDesign.xaml
    /// </summary>
    public partial class UserViewDesign : Window
    {
        Document Doc;
        List<ViewFamilyType> ViewFamilyTypeList;


        public ViewFamilyType SelectedViewFamilyType;


        public UserViewDesign(Document doc, List<ViewFamilyType> viewFamilyType)
        {
            Doc = doc;
            InitializeComponent();
            FormViewDesign.ItemsSource = viewFamilyType;
            FormViewDesign.DisplayMemberPath = "Name";

        }

        private void SaveSettings()
        {
            ViewFamilyTypeList = new FilteredElementCollector(Doc)
                .OfClass(typeof(ViewFamilyType))
                .WhereElementIsElementType()
                .Cast<ViewFamilyType>()
                .ToList();

            FormViewDesign.ItemsSource = ViewFamilyTypeList;
            FormViewDesign.DisplayMemberPath = "Name";

            SelectedViewFamilyType = FormViewDesign.SelectedItem as ViewFamilyType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Конец");

            DialogResult = true;

            Close();
        }
    }
}
