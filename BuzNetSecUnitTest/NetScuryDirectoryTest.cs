using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Configuration;
using BuzNetSec.Networking.Secury.IO;

namespace BuzNetSecUnitTest
{
    [TestClass]
    public class NetScuryDirectoryTest
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
            if (NetSecuryDirectory.SecuryDirectoryExists(_directoryDestinationPath, _sourceNC))
            {
                NetSecuryDirectory.SecuryDirectoryDelete(_directoryDestinationPath, _sourceNC);
            }
        }

        [TestMethod]
        public void SecuryDirectoryExistTest()
        {
            try
            {
                bool result = NetSecuryDirectory.SecuryDirectoryExists(_directoryDestinationPath, _sourceNC);

                Assert.AreEqual(true, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void SecuryDirectoryCreateTest()
        {
            try
            {
                NetSecuryDirectory.SecuryDirectoryCreate(_directoryDestinationPath, _sourceNC);

                bool result = NetSecuryDirectory.SecuryDirectoryExists(_directoryDestinationPath, _sourceNC);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }

        [TestMethod]
        public void SecuryDirectoryDeleteTest()
        {
            try
            {
                NetSecuryDirectory.SecuryDirectoryDelete(_directoryDestinationPath, _sourceNC);

                bool result = NetSecuryDirectory.SecuryDirectoryExists(_directoryDestinationPath, _sourceNC);

                Assert.AreEqual(false, result);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("{0},{1}", e.Message, e.StackTrace));
            }
        }
    }
}
