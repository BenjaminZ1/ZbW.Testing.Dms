using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
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
        string SerializeMetadataItem(MetadataItem metadataItem);
        MetadataItem DeserializeMetadataItem(string path);
        void SaveXml(string serializeXml, string path);
    }

    class XmlService : IXmlService
    {
        private readonly IFileSystem _fileSystem;

        public XmlService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public XmlService() : this (fileSystem: new FileSystem()){ }

        public string SerializeMetadataItem(MetadataItem metadataItem)
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
            try
            {
                using (Stream fs = (Stream) _fileSystem.FileStream.Create(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MetadataItem));
                    StreamReader reader = new StreamReader(fs);

                    var metadataItem = (MetadataItem) serializer.Deserialize(reader);
                    reader.Close();

                    return metadataItem;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SaveXml(string serializeXml, string path)
        {
            try
            {
                using (Stream fs = (Stream) _fileSystem.FileStream.Create(path, FileMode.CreateNew))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(serializeXml);
                    xmlDoc.Save(fs);
                    fs.Flush();
                    fs.Position = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}
