using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace BuzNetSec.Networking.Security.IO
{
    /// <summary>
    /// This class provide of secury methods to work with files 
    /// that be in network places and requires authentication.
    /// </summary>
    /// <remarks>
    /// Ange Buzany.
    /// </remarks>
    public static class File
    {        
        /// <summary>
        /// Copy a file of secury way between two network places.
        /// </summary>
        /// <param name="fileSrc">
        /// Path of source file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="fileDestination">
        /// Path of destination where will be copied the file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="ncRead">
        /// Network credentials of origin server.
        /// </param>
        /// <param name="ncWrite">
        /// Network credentials of destination server..
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static void Copy(string fileSrc, string fileDestination, NetworkCredential ncRead, NetworkCredential ncWrite)
        {
            try
            {
                DirectoryInfo diDirectorySrc = new DirectoryInfo(fileSrc);
                DirectoryInfo diDirectorDestination = new DirectoryInfo(fileDestination);

                using (new NetSecUseConnection(diDirectorySrc.Root.ToString(), ncRead))
                using (new NetSecUseConnection(diDirectorDestination.Root.ToString(), ncWrite))
                {
                    System.IO.File.Copy(fileSrc, fileDestination, true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Copy a file of secury way between a network place to local place.
        /// </summary>
        /// <param name="fileSrc">
        /// Path of source file (\\127.0.0.1\Example\File.doc).
        /// </param>
        /// <param name="fileDestination">
        /// Path of destination where will be copied the file (C:\Example\File.doc).
        /// </param>
        /// <param name="netCred">
        /// Network credentials of origin server.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static void Copy(string fileSrc, string fileDestination, NetworkCredential netCred, bool isReadNetCred)
        {
            try
            {
                DirectoryInfo diDirectorySrc = new DirectoryInfo(fileSrc);
                DirectoryInfo diDirectorDestination = new DirectoryInfo(fileDestination);

                if (isReadNetCred)
                {
                    using (new NetSecUseConnection(diDirectorySrc.Root.ToString(), netCred))
                    {
                        System.IO.File.Copy(fileSrc, fileDestination, true);
                    }
                    
                }
                else
                {
                    using (new NetSecUseConnection(diDirectorDestination.Root.ToString(), netCred))
                    {
                        System.IO.File.Copy(fileSrc, fileDestination, true);
                    }
                    
                }
              
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Delete a file of secury way in a network place.
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
        public static void Delete(string filePath, NetworkCredential ncRead)
        {
            try
            {
                FileInfo fiFileSrc = new FileInfo(filePath);

                using (new NetSecUseConnection(fiFileSrc.Directory.Root.FullName, ncRead))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verify if file exists of secury way in a network place.
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
        public static bool Exist(string filePath, NetworkCredential ncRead)
        {
            try
            {
                FileInfo fiFileSrc = new FileInfo(filePath);

                using (new NetSecUseConnection(fiFileSrc.Directory.Root.ToString(), ncRead))
                {
                    return System.IO.File.Exists(filePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
