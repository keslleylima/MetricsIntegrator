using MetricsIntegrator.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.IO
{
    public class MetricsFileManager
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsFileManager()
        {
            SmPath = string.Empty;
            MapPath = string.Empty;
            TestPathsPath = string.Empty;
            TestCasePath = string.Empty;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string SmPath { get; set; }
        public string MapPath { get; set; }
        public string TestPathsPath { get; set; }
        public string TestCasePath { get; set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void FindAll(string basePath)
        {
            string[] csvFilesPath = FileUtils.GetAllFilesFromDirectoryEndingWith(basePath, "csv");
            
            foreach (string csvPath in csvFilesPath)
            {
                if (csvPath.Contains("SCM_"))
                    SmPath = csvPath;

                if (csvPath.Contains("MAP_"))
                    MapPath = csvPath;

                if (csvPath.Contains("TestPath_"))
                    TestPathsPath = csvPath;

                if (csvPath.Contains("TestCase_"))
                    TestCasePath = csvPath;
            }
        }
    }
}
