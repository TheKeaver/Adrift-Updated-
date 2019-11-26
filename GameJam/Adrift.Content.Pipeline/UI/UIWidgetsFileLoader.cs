using Adrift.Content.Common.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Adrift.Content.Pipeline.UI
{
    public class UIWidgetsFileLoader
    {
        public static UIWidgetsFile Load(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(UIWidgetsFile));
            TextReader textReader = new StreamReader(filename);
            UIWidgetsFile file = (UIWidgetsFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
        }
    }
}
