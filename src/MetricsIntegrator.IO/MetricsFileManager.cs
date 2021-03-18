using MetricsIntegrator.Utils;
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
            SourceCodePath = string.Empty;
            MapPath = string.Empty;
            TestPathsPath = string.Empty;
            TestCasePath = string.Empty;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string SourceCodePath { get; set; }
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
                if (csvPath.Contains("SC_"))
                    SourceCodePath = csvPath;

                if (csvPath.Contains("MAP_"))
                    MapPath = csvPath;

                if (csvPath.Contains("TP_"))
                    TestPathsPath = csvPath;

                if (csvPath.Contains("TC_"))
                    TestCasePath = csvPath;
            }
        }

        public void SetFilesFromCLI(string[] args)
        {
            int idxParamStart = IsFlag(args[0]) ? 0 : 1;

            for (int i = idxParamStart; i < args.Length; i++)
            {
                if (IsFlag(args[i]))
                {
                    string flag = args[i];
                    i++;

                    switch (flag)
                    {
                        case "-sc":
                            SourceCodePath = args[i];
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
            Regex regex = new Regex("-(sc|map|tc|tp)");

            return regex.IsMatch(arg);
        }
    }
}
