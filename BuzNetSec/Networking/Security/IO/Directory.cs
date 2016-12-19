using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BuzNetSec.Networking.Security.IO
{
    /// <summary>
    /// This class provide of secury methods to work with
    /// direcotiries that be in network places and requires authentication.
    /// </summary>
    /// <remarks>
    /// Ange Buzany.
    /// </remarks>
    public static class Directory
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
        public static bool Exist(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectorySrc = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectorySrc.Root.FullName, ncRead))
                {
                    return System.IO.Directory.Exists(directoryPath);
                }
            }
            catch (Exception)
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
        public static void Create(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectroyPath = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectroyPath.Root.FullName, ncRead))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
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
        public static void Delete(string directoryPath, NetworkCredential ncRead)
        {
            try
            {
                DirectoryInfo diDirectroyPath = new DirectoryInfo(directoryPath);

                using (new NetSecUseConnection(diDirectroyPath.Root.FullName, ncRead))
                {
                    System.IO.Directory.Delete(directoryPath);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
