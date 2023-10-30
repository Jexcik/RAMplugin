﻿using System.IO;
using System.Xml.Serialization;

namespace RAM.ReinforcementColumnarFoundations
{
    public class RainforcementColumnarFoundationsSettingsT1
    {
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