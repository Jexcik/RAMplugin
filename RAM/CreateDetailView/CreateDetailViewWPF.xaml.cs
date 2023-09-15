using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RAM.CreateDetailView
{
    /// <summary>
    /// Логика взаимодействия для CreateDetailViewWPF.xaml
    /// </summary>
    public partial class CreateDetailViewWPF : Window
    {
        Document Doc;
        List<ViewFamilyType> ViewFamilyTypeList;
        List<ViewSection> ViewSectionTemplateList;

        public ViewFamilyType SelectedViewFamilyType;
        public bool UseTemplate;
        public ViewSection ViewSectionTemplate;
        public string SelectedBuildByName;
        public string SelectedUseToBuildName;
        public string SectionName;
        public ViewSheet SelectedViewSheet;
        public double Indent;
        public double ProjectionDepth;

        CreateDetailViewSettings CreateDetailViewSettingsItem;
        public CreateDetailViewWPF(Document doc, List<ViewSheet> viewSheetList)
        {
            Doc = doc;
            CreateDetailViewSettingsItem = new CreateDetailViewSettings().GetSettings();
            InitializeComponent();
            comboBox_PlaceOnSheet.ItemsSource = viewSheetList;
            comboBox_PlaceOnSheet.DisplayMemberPath = "Name";


            checkBox_UseTemplate.IsChecked = true;
            if (ViewSectionTemplateList.Count != 0)
            {
                comboBox_UseTemplate.SelectedItem = comboBox_UseTemplate.Items[0];
            }
        }
        private void checkBox_UseTemplate_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBox_UseTemplate.IsChecked)
            {
                comboBox_UseTemplate.IsEnabled = true;
                ViewSectionTemplateList = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ViewSection))
                    .Cast<ViewSection>()
                    .Where(vs => vs.IsTemplate == true)
                    .OrderBy(vft => vft.Name)
                    .ToList();

                comboBox_UseTemplate.ItemsSource = ViewSectionTemplateList;
                comboBox_UseTemplate.DisplayMemberPath = "Name";
                if (ViewSectionTemplateList.Count != 0)
                {
                    comboBox_UseTemplate.SelectedItem = comboBox_UseTemplate.Items[0];
                }
            }
            else
            {
                comboBox_UseTemplate.IsEnabled = false;
            }

        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            DialogResult = true;
            Close();
        }
        private void CreateDetailViewWPF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                SaveSettings();
                DialogResult = true;
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        }

        private void SaveSettings()
        {
            SelectedViewFamilyType = comboBox_SelectTypeSectionDetail.SelectedItem as ViewFamilyType;

            SelectedBuildByName = (groupBox_BuildBy.Content as System.Windows.Controls.Grid)
                .Children.OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked.Value == true)
                .Name;

            SelectedUseToBuildName = (groupBox_UseToBuild.Content as System.Windows.Controls.Grid)
                .Children.OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked.Value == true)
                .Name;

            UseTemplate = (bool)checkBox_UseTemplate.IsChecked;
            ViewSectionTemplate = comboBox_UseTemplate.SelectedItem as ViewSection;
            SelectedViewSheet = comboBox_PlaceOnSheet.SelectedItem as ViewSheet;

            double.TryParse(textBox_Indent.Text, out Indent);
            Indent = UnitUtils.ConvertToInternalUnits(Indent, UnitTypeId.Millimeters);

            double.TryParse(textBox_ProjectionDepth.Text, out ProjectionDepth);
            ProjectionDepth = UnitUtils.ConvertToInternalUnits(ProjectionDepth, UnitTypeId.Millimeters);

            SectionName = textBox_SectionName.Text;

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void UseToBuildCheckedChanged(object sender, RoutedEventArgs e)
        {
            string useToBuildSelectedName = (groupBox_UseToBuild.Content as System.Windows.Controls.Grid)
                .Children.OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked.Value == true)
                .Name;
            if (useToBuildSelectedName == "rbt_Section")
            {
                ViewFamilyTypeList = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ViewFamilyType))
                    .WhereElementIsElementType()
                    .Cast<ViewFamilyType>()
                    .Where(x => x.ViewFamily == ViewFamily.Section)
                    .OrderBy(x => x.Name)
                    .ToList(); ;
            }
            if (useToBuildSelectedName == "rbt_Uzel")
            {
                ViewFamilyTypeList = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ViewFamilyType))
                    .WhereElementIsElementType()
                    .Cast<ViewFamilyType>()
                    .Where(x => x.ViewFamily == ViewFamily.Detail)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            if (useToBuildSelectedName == "rbt_Sheet")
            {
                ViewFamilyTypeList = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ViewFamilyType))
                    .WhereElementIsElementType()
                    .Cast<ViewFamilyType>()
                    .Where(x => x.ViewFamily == ViewFamily.Elevation)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            comboBox_SelectTypeSectionDetail.ItemsSource = ViewFamilyTypeList;
            comboBox_SelectTypeSectionDetail.DisplayMemberPath = "Name";

        }
    }
}
