using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace BuzNetSec.Networking.Secury
{
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }

    /// <summary>
    /// Make a secury connection to a network resource
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NetSecuryAddConnection : IDisposable
    {
        private string _networkName;
        private string _ip;

        /// <summary>
        /// Constructor that open the conection wiht the server specified.
        /// </summary>
        /// <param name="networkName">
        /// IP or domain name.
        /// </param>
        /// <param name="credential">
        /// Credential of server with user and password.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public NetSecuryAddConnection(string networkName, NetworkCredential credential)
        {
            DirectoryInfo directoryPathNetwork = new DirectoryInfo(networkName);
            networkName = directoryPathNetwork.Root.FullName;
            _networkName = networkName;

            _ip = _networkName.Substring(2, _networkName.Length - 2);
            int firstDiag = _ip.IndexOf('\\');
            _ip = string.Format(@"\\{0}", _networkName.Substring(2, firstDiag));

            var netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credential.Domain)
                ? credential.UserName
                : string.Format(@"{0}\{1}", credential.Domain, credential.UserName);


            //Finish all connections
            WNetCancelConnection2(_ip, 0, true);

            //Establis connection
            var result = WNetAddConnection2(
                netResource,
                credential.Password,
                userName,
                0);


            if (result == 1219)
            {
                result = WNetCancelConnection2(networkName, 0, true);
                result = WNetAddConnection2(netResource, credential.Password, credential.UserName, 0);
            }

            if (result != 0)
            {
                string strErrMsg = string.Empty;

                switch (result)
                {
                    case 53:
                        strErrMsg = "The network path was not found.";
                        break;
                    case 67:
                        strErrMsg = "The network name cannot be found.";
                        break;
                    case 86:
                        strErrMsg = "Invalid UserName or Password for ProBiz server.";
                        break;
                    case 1219:
                        strErrMsg = "Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed.Close application to Disconnect all previous connections to the server or shared resource and try again.";
                        break;
                    default:
                        strErrMsg = "The network name cannot be found.";
                        break;
                }

                throw new Win32Exception(result, string.Format("NetSecuryConnection-Error: {0} Code: {1}", strErrMsg, result));
            }
        }

        ~NetSecuryAddConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

    }//End class NetSecAddConnection
}
