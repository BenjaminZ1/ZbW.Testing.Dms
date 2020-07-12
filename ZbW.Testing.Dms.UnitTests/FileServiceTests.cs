using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FakeItEasy;
using ZbW.Testing.Dms.Client.Services;
using System.IO.Abstractions.TestingHelpers;


namespace ZbW.Testing.Dms.UnitTests
{
    [TestFixture]
    class FileServiceTests
    {
        private const string TargetPath = @"C:\temp\DMS";
        private const string FilePath = @"C:\Temp\test.txt";
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        [Test]
        public void CopyFile_CheckDependency_CopyFileCalled()
        {
            //arrange
            const string newFileName = "test_neu.txt";
            var fileServiceMock = A.Fake<IFileService>();

            //act
            fileServiceMock.CopyFile(FilePath, newFileName);

            //assert
            A.CallTo(() => fileServiceMock.CopyFile(FilePath, newFileName)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CopyFile_Copy_IsSuccessful()
        {
            //arrange
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath );

            //act
            fileService.CopyFile(FilePath, newFileName);

            //assert
            Assert.That(fileSystem.File.Exists(savePath + @"\" + newFileName), Is.True);
        }

        [Test]
        public void CopyFile_Copy_FileNotFoundExceptionIsThrown()
        {
            //arrange
           
            const string nonExistingFilePath = @"C:\michgibtesnicht\test.txt";
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);

            //act
            void CopyFile() => fileService.CopyFile(nonExistingFilePath, newFileName);

            //assert
            Assert.Throws<System.IO.FileNotFoundException>(CopyFile,
                "System.IO.FileNotFoundException wird nicht ausgelöst.");
        }

        [Test]
        public void CopyFile_Copy_CreateFolderStructureIsSuccessful()
        {
            //arrange
           
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);

            //act
            fileService.CopyFile(FilePath, newFileName);

            //assert
            Assert.That(fileSystem.Directory.Exists(savePath), Is.True);
        }

        [Test]
        public void MoveFile_Move_IsSuccessful()
        {
            //arrange
           
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);

            //act
            fileService.MoveFile(FilePath, newFileName);

            //assert
            Assert.That(fileSystem.File.Exists(savePath + @"\" + newFileName), Is.True);
            Assert.That(fileSystem.File.Exists(FilePath), Is.False);
        }

        [Test]
        public void MoveFile_Move_FileNotFoundExceptionIsThrown()
        {
            //arrange
           
            const string nonExistingFilePath = @"C:\michgibtesnicht\test.txt";
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);

            //act
            void MoveFile() => fileService.MoveFile(nonExistingFilePath, newFileName);

            //assert
            Assert.Throws<System.IO.FileNotFoundException>(MoveFile,
                "System.IO.FileNotFoundException wird nicht ausgelöst.");
        }

        [Test]
        public void MoveFile_Move_CreateFolderStructureIsSuccessful()
        {
            //arrange
           
            const string newFileName = "flup.txt";
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {FilePath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);

            //act
            fileService.MoveFile(FilePath, newFileName);

            //assert
            Assert.That(fileSystem.Directory.Exists(savePath), Is.True);
        }


        [Test]
        public void GetAllFolderPaths_GetPaths_ReturnsCorrectResult()
        {
            //arrange
            
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem();
            var fileService = new FileService(fileSystem, savePath, TargetPath);
            string[] expectedResult = {savePath};

            //act
            var result = fileService.GetAllFolderPaths();

            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void GetAllXMLPaths_GetPaths_ReturnsCorrectResult()
        {
            //arrange

            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {savePath + @"\" + "test.xml", new MockFileData("Testing is...")},
                {savePath+ @"\" + "test2.xml", new MockFileData("Testing is not...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);
            string[] expectedResult = { savePath + @"\" + "test.xml", savePath + @"\" + "test2.xml" };

            //act
            var result = fileService.GetAllXmlPaths(savePath);

            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void GetFileExtension_GetExtension_ReturnsCorrectResult()
        {
            //arrange

            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {savePath + @"\" + "test.xml", new MockFileData("Testing is...")},
                {savePath+ @"\" + "test2.xml", new MockFileData("Testing is not...")}
            });
            var fileService = new FileService(fileSystem, savePath, TargetPath);
            string expectedResult = ".xml";

            //act
            var result = fileService.GetFileExtension(savePath + @"\" + "test.xml");

            //assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

}


