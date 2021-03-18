using MetricsIntegrator.Export;
using MetricsIntegrator.IO;
using MetricsIntegrator.Metric;
using MetricsIntegrator.Parser;
using MetricsIntegrator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MetricsIntegrator
{
    class Program
    {
        /// <param name="args">-scm path -map path -tc path -tp path</param>
        static void Main(string[] args)
        {
            // Find files
            MetricsFileManager metricsFileManager = new MetricsFileManager();

            if (args.Length > 1)
                metricsFileManager.SetFilesFromCLI(args);
            else
                metricsFileManager.FindAllFromDirectory(Directory.GetCurrentDirectory());

            MetricsParseManager metricsParseManager = new MetricsParseManager(metricsFileManager);
            metricsParseManager.Parse();

            // Export
            DoExportation(basePath, projectPath, mapping, listTestPath, listTestCase, scmParser);
        }

        private static void DoExportation(string basePath, string projectPath, Dictionary<string, string[]> mapping, List<MetricsContainer> listTestPath, List<MetricsContainer> listTestCase, SourceCodeMetricsParser scmParser)
        {
            string delimiter = ";";
            string TestPathFilePath = basePath + @"\TP_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            MetricsCSVExporter tpCSVExporter = new MetricsCSVExporter(TestPathFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestPath, delimiter);
            tpCSVExporter.Export();

            string TestCaseFilePath = basePath + @"\TC_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            MetricsCSVExporter tcCSVExporter = new MetricsCSVExporter(TestCaseFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestCase, delimiter);
            tcCSVExporter.Export();
        }
    }
}
