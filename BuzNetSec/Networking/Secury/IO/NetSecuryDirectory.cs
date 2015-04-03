using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BuzNetSec.Networking.Secury.IO
{
    /// <summary>
    /// This class provide of secury methods to work with
    /// direcotiries that be in network places and requires authentication.
    /// </summary>
    /// <remarks>
    /// Ange Buzany.
    /// </remarks>
    public static class NetSecuryDirectory
    {

        /// <summary>
        /// Verify if directory exists of secury way in a network place.
        /// </summary>
        /// <param name="filePath">
        /// Path of source file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="ncRead">
        /// Network credentials of origin server.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static bool SecuryDirectoryExists(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectorySrc = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectorySrc.Root.FullName, ncRead))
                {
                    return Directory.Exists(directoryPath);
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }//End method SecuryDirecctoryExists

        /// <summary>
        /// Make a directory of secury way in a network place.
        /// </summary>
        /// <param name="directoryPath">
        /// Path of source file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="ncRead">
        /// Network credentials of origin server.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static void SecuryDirectoryCreate(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectroyPath = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectroyPath.Root.FullName, ncRead))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }//End method SecuryDirecctoryCreate

        /// <summary>
        /// Delete a directory of secury way in a network place.
        /// </summary>
        /// <param name="directoryPath">
        /// Path of source file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="ncRead">
        /// Network credentials of origin server.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static void SecuryDirectoryDelete(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectroyPath = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectroyPath.Root.FullName, ncRead))
                {
                    Directory.Delete(directoryPath);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }//End method SecuryDirectoryDelete

    }//End class NetSecuryDirectory
}
