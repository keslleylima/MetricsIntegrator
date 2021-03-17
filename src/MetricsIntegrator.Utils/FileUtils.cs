using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Utils
{
    public class FileUtils
    {
        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public static string[] GetAllFilesFromDirectoryEndingWith(string directoryPath, string extensionName)
        {
            if (directoryPath == null)
                throw new ArgumentException("Directory path cannot be null");

            if (!Directory.Exists(directoryPath))
                throw new ArgumentException("Directory path does not exist or it is a file path");

            if ((extensionName == null) || (extensionName.Length == 0))
                throw new ArgumentException("Extension name cannot be empty");

            return Directory.GetFiles(
                directoryPath,
                $"*.{extensionName}",
                SearchOption.AllDirectories
            );
        }
    }
}
