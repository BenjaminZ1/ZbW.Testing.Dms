using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.Services
{
    using System.Xml;
    using System.Xml.Serialization;

    interface IXmlService
    {
        string SeralizeMetadataItem(MetadataItem metadataItem);
        MetadataItem DeserializeMetadataItem(string path);
        void SaveXml(string serializeXml, string path);
    }

    class XmlService : IXmlService
    {
        public string SeralizeMetadataItem(MetadataItem metadataItem)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MetadataItem));
            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter);

            xmlSerializer.Serialize(writer, metadataItem);
            var serializeXml = stringWriter.ToString();
            writer.Close();
            return serializeXml;
        }

        public MetadataItem DeserializeMetadataItem(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MetadataItem));
            StreamReader reader = new StreamReader(path);
            var metadataItem = (MetadataItem)serializer.Deserialize(reader);
            reader.Close();

            return metadataItem;
        }

        //Datei erstellen und Inhalt schreiben in FileServices auslagern
        public void SaveXml(string serializeXml, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serializeXml);
            xmlDoc.Save(path);
        }
    }
}
