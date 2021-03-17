using MetricsIntegrator.Export;
using MetricsIntegrator.Metric;
using MetricsIntegrator.Parser;
using MetricsIntegrator.Utils;
using System.Collections.Generic;

namespace MetricsIntegrator
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = @"C:\Users\william\Documents\workspace\vsstudio\MetricsIntegrator";
            string projectPath = basePath + @"\jopt"; //For example, @"D:\GitHub\TCmetricsGenerator\Projects\Cli"
            
            string[] csvFilesPath = FileUtils.GetAllFilesFromDirectoryEndingWith(projectPath, "csv");
            string smPath = string.Empty;
            
            
            string mapPath = string.Empty;
            string testPathsPath = string.Empty;
            string testCasePath = string.Empty;
            foreach (string csvPath in csvFilesPath)
            {
                if (csvPath.Contains("SCM_"))
                    smPath = csvPath;

                if (csvPath.Contains("MAP_"))
                    mapPath = csvPath;

                if (csvPath.Contains("TestPath_"))
                    testPathsPath = csvPath;

                if (csvPath.Contains("TestCase_"))
                    testCasePath = csvPath;
            }

            MappingMetricsParser mapParser = new MappingMetricsParser(mapPath);
            Dictionary<string, string[]> mapping = mapParser.Parse();

            string tpDelimiter = ";";
            TestPathMetricsParser tpParser = new TestPathMetricsParser(testPathsPath, tpDelimiter);
            List<MetricsContainer> listTestPath = tpParser.Parse();

            string tcDelimiter = ";";
            TestCaseMetricsParser tcParser = new TestCaseMetricsParser(testCasePath, tcDelimiter);
            List<MetricsContainer> listTestCase = tcParser.Parse();

            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(smPath);
            scmParser.Parse();


            string TestPathFilePath = basePath + @"\TP_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            TestPathCSVExporter tpCSVExporter = new TestPathCSVExporter(TestPathFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestPath);
            tpCSVExporter.Export();

            string TestCaseFilePath = basePath + @"\TC_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            string delimiter = ";";
            TestCaseCSVExporter tcCSVExporter = new TestCaseCSVExporter(TestCaseFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestCase, delimiter);
            tcCSVExporter.Export();
        }
    }
}
