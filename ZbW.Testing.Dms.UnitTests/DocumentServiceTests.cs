using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.UnitTests
{
    [TestFixture]
    class DocumentServiceTests
    {
        private const string DummyFilePath = @"C:\temp\dummy.xml";
        private const string DummyFolderPath = @"C:\temp\DMS\2020";
        [Test]
        public void AddFileToDMS_GenerateNewFileName_IsCalled()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);

            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var metadataItem = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);

            string guid = Guid.NewGuid().ToString();

            A.CallTo(() => fileServiceStub.GetFileExtension(DummyFilePath)).Returns(".xml");


            //act
            documentService.AddFileToDMS(metadataItem, DummyFilePath, false, guid);

            //assert
            A.CallTo(() => fileServiceStub.GetFileExtension(DummyFilePath)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void AddFileToDMS_CopyFile_IsCalled()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);

            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var metadataItem = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);

            string guid = Guid.NewGuid().ToString();
            string newFileName = guid + "_Content" + ".xml";

            A.CallTo(() => fileServiceStub.GetFileExtension(DummyFilePath)).Returns(".xml");

            //act
            documentService.AddFileToDMS(metadataItem, DummyFilePath, false, guid);

            //assert
            A.CallTo(() => fileServiceStub.CopyFile(DummyFilePath, newFileName)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void AddFileToDMS_MoveFile_IsCalled()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);

            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var metadataItem = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);

            string guid = Guid.NewGuid().ToString();
            string newFileName = guid + "_Content" + ".xml";
            A.CallTo(() => fileServiceStub.GetFileExtension(DummyFilePath)).Returns(".xml");

            //act
            documentService.AddFileToDMS(metadataItem, DummyFilePath, true, guid);

            //assert
            A.CallTo(() => fileServiceStub.MoveFile(DummyFilePath, newFileName)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllMetadataItems_GetItems_ReturnsCorrectResult()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);

            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var metadataItem = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);
            
            string guid = Guid.NewGuid().ToString();
            string newFileName = guid + "_Content" + ".xml";
            List<MetadataItem> expectedResult = new List<MetadataItem> {metadataItem};

            A.CallTo(() => fileServiceStub.GetFileExtension(DummyFilePath)).Returns(".xml");
            A.CallTo(() => fileServiceStub.GetAllFolderPaths()).Returns(new string[] {DummyFolderPath});
            A.CallTo(() => fileServiceStub.GetAllXmlPaths(DummyFolderPath)).Returns(new List<string>(new string[]{ DummyFilePath}));
            A.CallTo(() => xmlServiceStub.DeserializeMetadataItem(DummyFilePath)).Returns(metadataItem);

            //act
            var result = documentService.GetAllMetadataItems();

            //assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

    }
}




