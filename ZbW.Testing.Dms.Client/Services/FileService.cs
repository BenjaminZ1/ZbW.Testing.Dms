using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZbW.Testing.Dms.Client.Services
{
    interface IFileService
    {
        void CopyFile(string filePath, string newFileName);
        void MoveFile(string filePath, string newFileName);
        string GetFileExtension(string filePath);
        string[] GetAllFolderPaths();
        List<string> GetAllXmlPaths(string folderPath);
    }
    internal class FileService : IFileService
    {
        private readonly string _savePath;
        private readonly string _targetPath;
        private readonly IFileSystem _fileSystem;


        public FileService(IFileSystem fileSystem, string savePath, string targetPath)
        {
            _fileSystem = fileSystem;
            _savePath = savePath;
            _targetPath = targetPath;
            if (!CheckFolderStructure())
                CreateFolderStructure();
        }

        public FileService (string savePath, string targetPath) : this (fileSystem: new FileSystem(), savePath, targetPath) { }

        private void CreateFolderStructure()
        {
            _fileSystem.Directory.CreateDirectory(_savePath);
        }
        private bool CheckFolderStructure()
        {
            return _fileSystem.Directory.Exists(_savePath);
        }

        public void CopyFile(string filePath, string newFileName)
        {
            if (!CheckFolderStructure())
                CreateFolderStructure();
            var newFilePath = _savePath + @"\" + newFileName;
            try
            {
                _fileSystem.File.Copy(filePath, newFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void MoveFile(string filePath, string newFileName)
        {
            if (!CheckFolderStructure())
                CreateFolderStructure();
            var newFilePath = _savePath + @"\" + newFileName;
            try
            {
                _fileSystem.File.Move(filePath, newFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetFileExtension(string filePath)
        {
            return _fileSystem.Path.GetExtension(filePath);
        }

        public string[] GetAllFolderPaths()
        {
            return _fileSystem.Directory.GetDirectories(_targetPath);
        }

        public List<string> GetAllXmlPaths(string folderPath)
        {
            var xmlPaths = new List<string>();
            foreach (var file in _fileSystem.Directory.EnumerateFiles(folderPath, "*.xml"))
                xmlPaths.Add(file);

            return xmlPaths;
        }

    }
}