using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Utils
{
    class FileUtils
    {
        public static string[] ReadAllLines(string path)
        {
            if (path.Length == 0)
                return new string[0];

            return File.ReadAllLines(path);
        }

        public static string[] GetAllFilesFromDirectoryEndingWith(string directoryPath, string extensionName)
        {
            return Directory.GetFiles(
                directoryPath,
                $"*.{extensionName}",
                SearchOption.AllDirectories
            );
        }
    }
}
