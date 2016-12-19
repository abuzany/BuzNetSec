using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Configuration;
using BuzNetSec.Networking.Security.IO;

namespace BuzNetSecUnitTest.Networking.Security.IO
{
    [TestClass]
    public class DirectoryTest
    {
        private string _directorySourcePath = string.Empty;
        private string _directoryDestinationPath = string.Empty;

        private NetworkCredential _sourceNC = null;
        private NetworkCredential _destinationNC = null;

        [TestInitialize()]
        public void Initialize()
        {
            _directorySourcePath = ConfigurationManager.AppSettings["SourceDirectory"];
            _directoryDestinationPath = ConfigurationManager.AppSettings["DestinationDirectory"];

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
            // Delete directory if it was created by the test
            if (Directory.Exist(_directoryDestinationPath, _destinationNC))
            {
                Directory.Delete(_directoryDestinationPath, _destinationNC);
            }
        }

        [TestMethod]
        public void DirectoryExistTest()
        {
            try
            {
                bool result = Directory.Exist(_directoryDestinationPath, _destinationNC);

                Assert.AreEqual(true, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void DirectoryCreateTest()
        {
            try
            {
                Directory.Create(_directoryDestinationPath, _destinationNC);

                bool result = Directory.Exist(_directoryDestinationPath, _destinationNC);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void DirectoryDeleteTest()
        {
            try
            {
                Directory.Delete(_directoryDestinationPath, _destinationNC);

                bool result = Directory.Exist(_directoryDestinationPath, _destinationNC);

                Assert.AreEqual(false, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }
    }
}
