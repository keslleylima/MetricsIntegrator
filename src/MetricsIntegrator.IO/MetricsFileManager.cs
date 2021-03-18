using MetricsIntegrator.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        public void FindAllFromDirectory(string basePath)
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

        public void SetFilesFromCLI(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                if (IsFlag(args[i]))
                {
                    string flag = args[i];
                    i++;

                    switch (flag)
                    {
                        case "-scm":
                            SmPath = args[i];
                            break;
                        case "-map":
                            MapPath = args[i];
                            break;
                        case "-tc":
                            TestCasePath = args[i];
                            break;
                        case "-tp":
                            TestPathsPath = args[i];
                            break;
                    }
                }
            }
        }

        private bool IsFlag(string arg)
        {
            Regex regex = new Regex("-(scm|map|tc|tp)");

            return regex.IsMatch(arg);
        }
    }
}
