using MetricsIntegrator.Export;
using MetricsIntegrator.IO;
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

            MetricsFileManager metricsFileManager = new MetricsFileManager();
            metricsFileManager.FindAll(projectPath);

            MappingMetricsParser mapParser = new MappingMetricsParser(metricsFileManager.MapPath);
            Dictionary<string, string[]> mapping = mapParser.Parse();

            string tpDelimiter = ";";
            TestPathMetricsParser tpParser = new TestPathMetricsParser(metricsFileManager.TestPathsPath, tpDelimiter);
            List<MetricsContainer> listTestPath = tpParser.Parse();

            string tcDelimiter = ";";
            TestCaseMetricsParser tcParser = new TestCaseMetricsParser(metricsFileManager.TestCasePath, tcDelimiter);
            List<MetricsContainer> listTestCase = tcParser.Parse();

            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(metricsFileManager.SmPath);
            scmParser.Parse();

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
