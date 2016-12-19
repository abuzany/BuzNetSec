using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Configuration;
using BuzNetSec.Networking.Security.IO;

namespace BuzNetSecUnitTest.Networking.Security.IO
{
    [TestClass]
    class FileTest
    {
        private string _fileSourcePath = string.Empty;
        private string _fileDestinationPath = string.Empty;

        private NetworkCredential _sourceNC = null;
        private NetworkCredential _destinationNC = null;

        [TestInitialize()]
        public void Initialize()
        {
            _fileSourcePath = ConfigurationManager.AppSettings["SourceFile"];
            _fileDestinationPath = ConfigurationManager.AppSettings["DestinationFile"];

            // Create network credentials for server authentication
            _sourceNC = new NetworkCredential();
            _sourceNC.Domain = ConfigurationManager.AppSettings["SourceIP"];
            _sourceNC.UserName = ConfigurationManager.AppSettings["SourceUser"];
            _sourceNC.Password = ConfigurationManager.AppSettings["SourcePassword"];

            _destinationNC = new NetworkCredential();
            _destinationNC.Domain = ConfigurationManager.AppSettings["DestinationIP"];
            _destinationNC.UserName = ConfigurationManager.AppSettings["DestinationUser"];
            _destinationNC.Password = ConfigurationManager.AppSettings["DestinationPassword"];
        }

        [TestCleanup()]
        public void Cleanup()
        {
            // Delete file if it was created by the test
            if (File.Exist(_fileDestinationPath, _destinationNC))
            {
                File.Delete(_fileSourcePath, _destinationNC);
            }
        }

        [TestMethod]
        public void FileExistTest()
        {
            try
            {
                bool result = File.Exist(_fileDestinationPath, _destinationNC);

                Assert.AreEqual(true, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void FileCopyTest()
        {
            try
            {
                File.Copy(_fileSourcePath, _fileSourcePath, _sourceNC, _destinationNC);

                bool result = File.Exist(_fileDestinationPath, _destinationNC);

                Assert.AreEqual(true, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void FileDeleteTest()
        {
            try
            {
                File.Delete(_fileDestinationPath, _destinationNC);

                bool result = File.Exist(_fileDestinationPath, _destinationNC);

                Assert.AreEqual(false, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }
    }
}
