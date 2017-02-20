using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BarProject.DesktopApplication.Common.Utils
{
    public static class ConfigHelper
    {
        public static int Workstation { get; set; }
        public static int Spot { get; set; }
        static ConfigHelper()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(folder, "BarProject");
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);
            var filePath = Path.Combine(appFolder, "config.xml");
            if (!File.Exists(filePath))
            {
                CreateConfig(filePath);
            }
            else
            {

                var data = GetData(filePath);
                Workstation = data.Item1;
                Spot = data.Item2;
            }


        }

        private static Tuple<int, int> GetData(string filePath, int i = 0)
        {
            if (i > 10)
            {
                return new Tuple<int, int>(1, 1);
            }
            var doc = XDocument.Load(filePath);
            if (doc.Root == null)
            {
                File.Delete(filePath);
                CreateConfig(filePath);
                return GetData(filePath, i + 1);
            }
            var wrokstation = (doc.Root?.Elements()).FirstOrDefault(x => x.Name.LocalName == "spot");
            var spot = (doc.Root?.Elements()).FirstOrDefault(x => x.Name.LocalName == "spot");
            if (spot == null || wrokstation == null || spot.IsEmpty || wrokstation.IsEmpty)
            {
                File.Delete(filePath);
                CreateConfig(filePath);
                return GetData(filePath, i + 1);
            }
            var spotId = int.Parse(spot.Value);
            var workstationId = int.Parse(spot.Value);
            return new Tuple<int, int>(workstationId, spotId);
        }
        private static void CreateConfig(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            var configElement = doc.CreateElement("config");
            var workstation = doc.CreateElement("workstation");
            var spot = doc.CreateElement("spot");
            workstation.InnerText = "1";
            spot.InnerText = "1";
            configElement.AppendChild(workstation);
            configElement.AppendChild(spot);
            doc.AppendChild(configElement);
            using (var file = File.CreateText(filePath))
            {
                doc.Save(file);
            }
            Workstation = 1;
        }

    }
}
