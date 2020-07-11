using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.Services
{
    internal class DocumentService : BindableBase
    {
        private readonly string _savePath;
        private readonly string _targetPath;
        private readonly IFileService _fileService;
        private readonly IXmlService _xmlService;
        private List<MetadataItem> _metadataItems;
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        public List<MetadataItem> MetadataItems
        {
            get => _metadataItems;

            set => SetProperty(ref _metadataItems, value);
        }

        public DocumentService()
        {
            _targetPath = ConfigurationManager.AppSettings["RepositoryDir"];
            _savePath = _targetPath + @"\" + _currentYear;
            _fileService = new FileService(_savePath, _targetPath);
            _xmlService = new XmlService();
        }

        public DocumentService(IFileService fileService, IXmlService xmlService)
        {
            _targetPath = ConfigurationManager.AppSettings["RepositoryDir"];
            _savePath = _targetPath + @"\" + _currentYear;
            _fileService = fileService;
            _xmlService = xmlService;
        }

        public void AddFileToDMS(MetadataItem metadataItem, string filePath, bool isRemoveFileEnabled)
        {
            string guid = Guid.NewGuid().ToString();
            string newFileName = GenerateNewFileName(filePath, guid);
            string newXmlName = GenerateNewXMLName(guid);
            string newXmlPath = _savePath + @"\" + newXmlName;

            if (isRemoveFileEnabled)
            {
                _fileService.MoveFile(filePath, newFileName);
                metadataItem.FilePath = _savePath + @"\" + newFileName;
                _xmlService.SaveXml(_xmlService.SeralizeMetadataItem(metadataItem), newXmlPath);
            }
            else
            {
                _fileService.CopyFile(filePath, newFileName);
                metadataItem.FilePath = _savePath + @"\" + newFileName;
                _xmlService.SaveXml(_xmlService.SeralizeMetadataItem(metadataItem), newXmlPath);
            }
        }

        private string GenerateNewFileName(string filePath, string guid)
        {
            string newFileName = guid + "_Content" + _fileService.GetFileExtension(filePath);
            return newFileName;
        }

        private string GenerateNewXMLName(string guid)
        {
            string newFileName = guid + "_Metadata.xml";
            return newFileName;
        }

        public List<MetadataItem> GetAllMetadataItems()
        {
            var folderPaths = _fileService.GetAllFolderPaths();
            var xmlPathsFromAllFolders = new List<string>();
            var metadataItemList = new List<MetadataItem>();

            foreach (var folderPath in folderPaths)
            {
                var xmlPathsFromOneFolder = _fileService.GetAllXmlPaths(folderPath);
                xmlPathsFromAllFolders.AddRange(xmlPathsFromOneFolder);
            }

            foreach (var xmlPath in xmlPathsFromAllFolders)
                metadataItemList.Add(_xmlService.DeserializeMetadataItem((string)xmlPath));

            MetadataItems = metadataItemList;
            return MetadataItems;
        }

        public List<MetadataItem> FilterAllMetadataItems(string suchbegriff, string type)
        {
            if (string.IsNullOrEmpty(suchbegriff) && string.IsNullOrEmpty(type))
                return MetadataItems;
            else if (string.IsNullOrEmpty(suchbegriff))
                suchbegriff = "";

            List<MetadataItem> filteredMetaDataItems = new List<MetadataItem>();
            foreach (var metadataItem in MetadataItems)
            {
                if ((metadataItem.Bezeichnung.Contains(suchbegriff) | metadataItem.Stichwoerter.Contains(suchbegriff))
                    && (string.IsNullOrEmpty(type) || metadataItem.SelectedTypItem.Equals(type)))
                {
                    filteredMetaDataItems.Add(metadataItem);
                }
            }
            return filteredMetaDataItems;
        }

        public void OpenDMSFile(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }
    }
}
