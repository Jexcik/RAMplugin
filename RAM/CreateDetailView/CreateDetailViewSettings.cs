using System.IO;
using System.Xml.Serialization;

namespace RAM.CreateDetailView
{
    public class CreateDetailViewSettings
    {
        public string SelectedViewFamilyTypeName { get; set; }
        public bool UseTemplate { get; set; }
        public string ViewSectionTemplateName { get; set; }
        public string SelectedBuildByName { get; set; }
        public string SelectedUseToBuildName { get; set; }
        public string Indent { get; set; }
        public string IndentUp { get; set; }
        public string IndentDown { get; set; }
        public string ProjectionDepth { get; set; }
        public string CurveNumberOfSegments { get; set; }
        public string SelectedViewSheetName { get; set; }

        public CreateDetailViewSettings GetSettings()
        {
            CreateDetailViewSettings createDetailViewSettings = null;
            string assemblyPathAll = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string fileName = "CreateDetailViewSettings.xml";
            string assemblyPath = assemblyPathAll.Replace("RAM.dll", fileName);

            if (File.Exists(assemblyPath))
            {
                using (FileStream fs = new FileStream(assemblyPath, FileMode.Open))
                {
                    XmlSerializer xSer = new XmlSerializer(typeof(CreateDetailViewSettings));
                    createDetailViewSettings = xSer.Deserialize(fs) as CreateDetailViewSettings;
                    fs.Close();
                }
            }
            else
            {
                createDetailViewSettings = null;
            }

            return createDetailViewSettings;
        }
        public void SaveSettings()
        {
            string assemblyPathAll = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string fileName = "CreateDetailViewSettings.xml";
            string assemblyPath = assemblyPathAll.Replace("RAM.dll", fileName);

            if (File.Exists(assemblyPath))
            {
                File.Delete(assemblyPath);
            }

            using (FileStream fs = new FileStream(assemblyPath, FileMode.Create))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(CreateDetailViewSettings));
                xSer.Serialize(fs, this);
                fs.Close();
            }
        }
    }
}