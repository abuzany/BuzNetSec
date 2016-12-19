using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace BuzNetSec.Networking.Security
{
    /// <summary>
    /// Make a secury connection to a network resource
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NetSecUseConnection : IDisposable
    {
        string _remoteUNC;

        #region Consts
        const int RESOURCE_CONNECTED = 0x00000001;
        const int RESOURCE_GLOBALNET = 0x00000002;
        const int RESOURCE_REMEMBERED = 0x00000003;
        const int RESOURCETYPE_ANY = 0x00000000;
        const int RESOURCETYPE_DISK = 0x00000001;
        const int RESOURCETYPE_PRINT = 0x00000002;
        const int RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        const int RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        const int RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        const int RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        const int RESOURCEDISPLAYTYPE_GROUP = 0x00000005;
        const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        const int RESOURCEUSAGE_CONTAINER = 0x00000002;
        const int CONNECT_INTERACTIVE = 0x00000008;
        const int CONNECT_PROMPT = 0x00000010;
        const int CONNECT_REDIRECT = 0x00000080;
        const int CONNECT_UPDATE_PROFILE = 0x00000001;
        const int CONNECT_COMMANDLINE = 0x00000800;
        const int CONNECT_CMD_SAVECRED = 0x00001000;
        const int CONNECT_LOCALDRIVE = 0x00000100;
        #endregion

        #region Errors
        const int NO_ERROR = 0;
        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_NEWORK_PATH_NOT_FOUND = 53;
        const int ERROR_ALREADY_ASSIGNED = 85;
        const int ERROR_INVALID_USERNAME_OR_PASSWORD = 86;
        const int ERROR_BAD_DEVICE = 1200;
        const int ERROR_BAD_NET_NAME = 67;
        const int ERROR_BAD_PROVIDER = 1204;
        const int ERROR_CANCELLED = 1223;
        const int ERROR_EXTENDED_ERROR = 1208;
        const int ERROR_INVALID_ADDRESS = 487;
        const int ERROR_INVALID_PARAMETER = 87;
        const int ERROR_INVALID_PASSWORD = 1216;
        const int ERROR_MORE_DATA = 234;
        const int ERROR_NO_MORE_ITEMS = 259;
        const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        const int ERROR_MULTIPLE_CONECTIONS = 1219;
        const int ERROR_NO_NETWORK = 1222;
        const int ERROR_BAD_PROFILE = 1206;
        const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        const int ERROR_DEVICE_IN_USE = 2404;
        const int ERROR_NOT_CONNECTED = 2250;
        const int ERROR_OPEN_FILES = 2401;
        #endregion

        private static ErrorClass[] ERROR_LIST = new ErrorClass[] {
			new ErrorClass(ERROR_ACCESS_DENIED, "Error: Access Denied"), 
			new ErrorClass(ERROR_ALREADY_ASSIGNED, "Error: Already Assigned"), 
			new ErrorClass(ERROR_BAD_DEVICE, "Error: Bad Device"), 
			new ErrorClass(ERROR_BAD_NET_NAME, "Error: Bad Net Name"), 
			new ErrorClass(ERROR_BAD_PROVIDER, "Error: Bad Provider"), 
			new ErrorClass(ERROR_CANCELLED, "Error: Cancelled"), 
			new ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"), 
			new ErrorClass(ERROR_INVALID_ADDRESS, "Error: Invalid Address"), 
			new ErrorClass(ERROR_INVALID_PARAMETER, "Error: Invalid Parameter"), 
			new ErrorClass(ERROR_INVALID_PASSWORD, "Error: Invalid Password"), 
			new ErrorClass(ERROR_MORE_DATA, "Error: More Data"), 
			new ErrorClass(ERROR_NO_MORE_ITEMS, "Error: No More Items"), 
			new ErrorClass(ERROR_NO_NET_OR_BAD_PATH, "Error: No Net Or Bad Path"), 
			new ErrorClass(ERROR_NO_NETWORK, "Error: No Network"), 
			new ErrorClass(ERROR_BAD_PROFILE, "Error: Bad Profile"), 
			new ErrorClass(ERROR_CANNOT_OPEN_PROFILE, "Error: Cannot Open Profile"), 
			new ErrorClass(ERROR_DEVICE_IN_USE, "Error: Device In Use"), 
			new ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"), 
			new ErrorClass(ERROR_NOT_CONNECTED, "Error: Not Connected"), 
			new ErrorClass(ERROR_OPEN_FILES, "Error: Open Files"), 
			new ErrorClass(ERROR_MULTIPLE_CONECTIONS,"Error: Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed.Close application to Disconnect all previous connections to the server or shared resource and try again"),
			new ErrorClass(ERROR_NEWORK_PATH_NOT_FOUND, "Error: The network path was not found"), 
			new ErrorClass(ERROR_INVALID_USERNAME_OR_PASSWORD, "Error: Invalid UserName or Password for ProBiz server"), 
		};

        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult
            );

        [DllImport("Mpr.dll")]
        private static extern int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            bool fForce
            );

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 0;
            public int dwDisplayType = 0;
            public int dwUsage = 0;
            public string lpLocalName = string.Empty;
            public string lpRemoteName = string.Empty;
            public string lpComment = string.Empty;
            public string lpProvider = string.Empty;
        }

        private struct ErrorClass
        {
            public int num;
            public string message;

            public ErrorClass(int num, string message)
            {
                this.num = num;
                this.message = message;
            }
        }

        /// <summary>
        /// Make a instance of NetSecUseConnection
        /// </summary>
        public NetSecUseConnection()
        {
        }

        /// <summary>
        /// Ctor when is instanced automatically
        /// establish a connection with the path
        /// </summary>
        /// <param name="username">
        /// Username of remote server.
        /// </param>
        /// <param name="password">
        /// Password of remote server.
        /// </param>
        public NetSecUseConnection(string remoteUNC, NetworkCredential credential)
        {
            this._remoteUNC = remoteUNC;

            this.DisconnectRemote(remoteUNC);
            this.ConnectToRemote(remoteUNC, credential.UserName, credential.Password);
        }

        /// <summary>
        /// Make a connection to a network resource
        /// </summary>
        /// <param name="remoteUNC">
        /// Network Path.
        /// </param>
        /// <param name="username">
        /// Username of remote server.
        /// </param>
        /// <param name="password">
        /// Password of remote server.
        /// </param>
        public void ConnectToRemote(string remoteUNC, string username, string password)
        {
            this._remoteUNC = remoteUNC;

            ConnectToRemote(remoteUNC, username, password, false);
        }//End method ConnectToRemote

        /// <summary>
        /// Make a connection to a network resource
        /// </summary>
        /// <param name="remoteUNC">
        /// Network Path.
        /// </param>
        /// <param name="username">
        /// Username of remote server.
        /// </param>
        /// <param name="password">
        /// Password of remote server.
        /// </param>
        /// <param name="promptUser">
        /// Open a prompt if is true.
        /// </param>
        public void ConnectToRemote(string remoteUNC, string username, string password, bool promptUser)
        {
            this._remoteUNC = remoteUNC;

            NETRESOURCE nr = new NETRESOURCE();
            nr.dwType = RESOURCETYPE_DISK;
            nr.lpRemoteName = remoteUNC;

            int ret;

            // If promptUser is tru launch an UI
            // to enter credential
            if (promptUser)
            {
                ret = WNetUseConnection(IntPtr.Zero, nr, "", "", CONNECT_INTERACTIVE | CONNECT_PROMPT, null, null, null);
            }
            else
            {
                ret = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);
            }

            // Multiple connections to a server or shared resource by the same user, 
            // using more than one user name, are not allowed.
            // Close application to Disconnect all previous connections to the server or shared resource and try again
            if (ret == ERROR_MULTIPLE_CONECTIONS)
            {
                WNetCancelConnection2(_remoteUNC, CONNECT_UPDATE_PROFILE, false);

                if (promptUser)
                {
                    ret = WNetUseConnection(IntPtr.Zero, nr, "", "", CONNECT_INTERACTIVE | CONNECT_PROMPT, null, null, null);
                }
                else
                {
                    ret = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);
                }
            }

            if (ret != NO_ERROR)
            {
                string strErrMsg = string.Empty;

                strErrMsg = GetErrorForNumber(ret);

                throw new Win32Exception(ret, strErrMsg);
            }
        }

        /// <summary>
        /// Disconnect to user of network resource
        /// </summary>
        /// <param name="remoteUNC">
        /// Network Path.
        /// </param>
        public void DisconnectRemote(string remoteUNC)
        {
            int ret = WNetCancelConnection2(remoteUNC, CONNECT_UPDATE_PROFILE, false);

            if (ret != NO_ERROR)
            {
                string strErrMsg = string.Empty;

                strErrMsg = GetErrorForNumber(ret);

                throw new Win32Exception(ret, strErrMsg);
            }
        }

        private string GetErrorForNumber(int errNum)
        {
            foreach (ErrorClass er in ERROR_LIST)
            {
                if (er.num == errNum) return er.message;
            }
            return string.Format("Error: Unknown, {0}", errNum);
        }

        ~NetSecUseConnection()
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
            DisconnectRemote(_remoteUNC);
        }
    }
}