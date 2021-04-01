using MetricsIntegrator.Utils;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MetricsIntegrator.IO
{
    /// <summary>
    ///     Responsible for finding metrics files.
    /// </summary>
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
        /// <summary>
        ///     Find metrics files, from a given directory path, with the 
        ///     following prefixes:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>'SC_'</term>
        ///             <description>Source code metrics path</description>
        ///         </item>
        ///         <item>
        ///             <term>'MAP_'</term>
        ///             <description>Mapping path</description>
        ///         </item>
        ///         <item>
        ///             <term>'TC_'</term>
        ///             <description>Test case metrics path</description>
        ///         </item>
        ///         <item>
        ///             <term>'TP_'</term>
        ///             <description>Test paths metrics path</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// 
        /// <param name="dirPath">Directory path</param>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If directory path is null or invalid
        /// </exception>
        public void FindAllFromDirectory(string dirPath)
        {
            if (dirPath == null)
                throw new ArgumentException("Directory path cannot be null");

            if (!Directory.Exists(dirPath))
                throw new ArgumentException("Invalid directory path");

            string[] csvFilesPath = FileUtils.GetAllFilesFromDirectoryEndingWith(dirPath, "csv");
            
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

        /// <summary>
        ///     Defines metrics files from cli following the following 
        ///     structure:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>-sc</term>
        ///             <description>Source code metrics file flag</description>
        ///         </item>
        ///         <item>
        ///             <term>-map</term>
        ///             <description>Mapping file flag</description>
        ///         </item>
        ///         <item>
        ///             <term>-tc</term>
        ///             <description>Test case metrics file flag</description>
        ///         </item>
        ///         <item>
        ///             <term>-tp</term>
        ///             <description>Test paths metrics file flag</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// 
        /// <example>
        ///     <code>-sc sourcecode.csv -map map.csv -tc testcase.csv -tp testpath.csv</code>
        /// </example>
        /// 
        /// <param name="args">CLI arguments</param>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If args is null or empty.
        /// </exception>
        public void SetFilesFromCLI(string[] args)
        {
            if ((args == null) || args.Length == 0)
                throw new ArgumentException("Arguments cannot be empty");

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
