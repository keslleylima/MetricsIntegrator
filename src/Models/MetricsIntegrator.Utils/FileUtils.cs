using System;
using System.IO;

namespace MetricsIntegrator.Utils
{
    /// <summary>
    ///     Responsible for grouping auxiliary methods of manipulating files.
    /// </summary>
    public class FileUtils
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private FileUtils()
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        /// <summary>
        ///     Searches for all files in a directory that end with a certain
        ///     extension.
        /// </summary>
        /// 
        /// <param name="dirPath">Directory path</param>
        /// <param name="extensionName">Extension (without dot)</param>
        /// 
        /// <returns>List of files found</returns>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If directory path is null, does not exit or if extension name
        ///     is null or empty.
        /// </exception>
        public static string[] GetAllFilesFromDirectoryEndingWith(string dirPath, string extensionName)
        {
            if (dirPath == null)
                throw new ArgumentException("Directory path cannot be null");

            if (!Directory.Exists(dirPath))
                throw new ArgumentException("Directory path does not exist or it is a file path.\n" + dirPath);

            if ((extensionName == null) || (extensionName.Length == 0))
                throw new ArgumentException("Extension name cannot be empty");

            return Directory.GetFiles(
                dirPath,
                $"*.{extensionName}",
                SearchOption.AllDirectories
            );
        }
    }
}
