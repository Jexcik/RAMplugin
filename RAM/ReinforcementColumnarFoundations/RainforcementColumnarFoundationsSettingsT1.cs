using System.IO;
using System.Xml.Serialization;

namespace RAM.ReinforcementColumnarFoundations
{
    public class RainforcementColumnarFoundationsSettingsT1
    {
        public string Form01Name { get; set; }
        public string Form26Name { get; set; }
        public string Form11Name { get; set; }
        public string Form51Name { get; set; }
        public string RebarHookTypeForStirrupName { get; set; }

        public string FirstMainBarTapeName { get; set; }
        public string FirstStirrupBarTapeName { get; set; }
        public string SupracolumnRebarBarCoverTypeName { get; set; }
        public string BottomRebarCoverTypeName { get; set; }

        public RainforcementColumnarFoundationsSettingsT1 GetSettings()
        {
            RainforcementColumnarFoundationsSettingsT1 rainforcementColumnarFoundationsSettingsT1 = null;
            string assemblyPathAll = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string fileName = "RainforcementColumnarFoundationsSettingsT1.xml";
            string assemblyPath = assemblyPathAll.Replace("RAM.dll", fileName);

            if (File.Exists(assemblyPath))
            {
                using (FileStream fs = new FileStream(assemblyPath, FileMode.Open))
                {
                    XmlSerializer xSer = new XmlSerializer(typeof(RainforcementColumnarFoundationsSettingsT1));
                    rainforcementColumnarFoundationsSettingsT1 = xSer.Deserialize(fs) as RainforcementColumnarFoundationsSettingsT1;
                    fs.Close();
                }
            }
            else
            {
                rainforcementColumnarFoundationsSettingsT1 = null;
            }

            return rainforcementColumnarFoundationsSettingsT1;
        }
        public void SaveSettings()
        {
            string assemblyPathAll = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string fileName = "RainforcementColumnarFoundationsSettingsT1.xml";
            string assemblyPath = assemblyPathAll.Replace("RAM.dll", fileName);

            if (File.Exists(assemblyPath))
            {
                File.Delete(assemblyPath);
            }
            using (FileStream fs = new FileStream(assemblyPath, FileMode.Create))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(RainforcementColumnarFoundationsSettingsT1));
                xSer.Serialize(fs, this);
                fs.Close();
            }
        }

    }
}