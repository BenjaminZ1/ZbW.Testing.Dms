using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
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
    class XmlServiceTests
    {
        private const string TargetPath = @"C:\temp\DMS";
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        [Test]
        public void SerializeMetadataItem_Serialize_ReturnsCorrectResult()
        {
            //arrange
            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var metadataItem = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);
            var XmlService = new XmlService();
            string expectedResult =  "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>";

            //act
            var result = XmlService.SerializeMetadataItem(metadataItem);

            //assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void DeserializeMetadataItem_Deserialize_ReturnsCorrectResult()
        {
            //arrange
            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var expectedResult = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {savePath + @"\" + "test.xml", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>")},
                {savePath+ @"\" + "test2.xml", new MockFileData("Testing is not...")}
            });
            var XmlService = new XmlService(fileSystem);

            //act
            var result = XmlService.DeserializeMetadataItem(savePath + @"\" + "test.xml");

            //assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void DeserializeMetadataItem_Deserialize_ThrowsDirectoryNotFoundException()
        {
            //arrange
            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var expectedResult = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);
            string savePath = TargetPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem();
            var XmlService = new XmlService(fileSystem);

            //act
            void DeserializeMetadataItem() => XmlService.DeserializeMetadataItem(savePath + @"\" + "test.xml");

            //assert
            Assert.Throws<DirectoryNotFoundException>(DeserializeMetadataItem, "Die Exception wird nicht ausgelöst!");
        }

        [Test]
        public void SaveXML_Save_IsCalled()
        {
            //arrange
            var XmlServiceMock = A.Fake<IXmlService>();
            string savePath = TargetPath + @"\" + _currentYear;
            string filePath = savePath + @"\" + "test.xml";
            string serializeXml =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>";

            //act
            XmlServiceMock.SaveXml(serializeXml, filePath);

            //assert
            A.CallTo(() => XmlServiceMock.SaveXml(serializeXml, filePath)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SaveXML_Save_ThrowsDirectoryNotFoundException()
        {
            //arrange
            var erfassungsDatum = new DateTime(2020, 01, 01);
            DateTime? valutaDatum = new DateTime(2020, 02, 02);
            var expectedResult = new MetadataItem("TestUser", "TestFile", erfassungsDatum, "Vertäge", "Ich bin ein Test", valutaDatum);
            string savePath = TargetPath + @"\" + _currentYear + @"\test.xml";
            string serializeXml =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>";
            var fileSystem = new MockFileSystem();
            var XmlService = new XmlService(fileSystem);

            //act
            void SaveXml() => XmlService.SaveXml(serializeXml, savePath);

            //assert
            Assert.Throws<DirectoryNotFoundException>(SaveXml, "Die Exception wird nicht ausgelöst!");

        }

        [Test]
        public void SaveXML_Save_SavesSuccessfully()
        {
            //arrange
            string savePath = TargetPath + @"\" + _currentYear + @"\test.xml";
            string serializeXml =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>";
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(TargetPath + @"\" + _currentYear);
            var XmlService = new XmlService(fileSystem);

            //act
            XmlService.SaveXml(serializeXml, savePath);

            //assert
            Assert.That(fileSystem.File.Exists(savePath));

        }
    }
}
