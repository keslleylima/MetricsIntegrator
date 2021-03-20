using MetricsIntegrator.Data;
using System;

namespace MetricsIntegrator.Export
{
    public class MetricsExporterFactory
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly string DELIMITER = ";";


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsExporterFactory()
        {
        }


        //---------------------------------------------------------------------
        //		Factories
        //---------------------------------------------------------------------
        public static MetricsCSVExporter CreateTestCaseCSVExporter(string outputDir, string projectName, MetricsContainer metrics)
        {
            CheckRequiredFields(outputDir, projectName, metrics);

            if (!metrics.HasTestCaseMetrics())
                throw new ArgumentException("There are no test case metrics");

            string outputPath = outputDir + @"\TC_dataset_resulting_" + projectName + ".csv";

            return new MetricsCSVExporter.Builder(outputPath)
                .Mapping(metrics.Mapping)
                .SourceCodeMetrics(metrics.SourceCodeMetrics)
                .TestCodeMetrics(metrics.TestCodeMetrics)
                .BaseMetrics(metrics.TestCaseMetrics)
                .UsingDelimiter(DELIMITER)
                .Build(); ;
        }

        public static MetricsCSVExporter CreateTestPathCSVExporter(string outputDir, string projectName, MetricsContainer metrics)
        {
            CheckRequiredFields(outputDir, projectName, metrics);

            if (!metrics.HasTestPathMetrics())
                throw new ArgumentException("There are no test path metrics");

            string outputPath = outputDir + @"\TP_dataset_resulting_" + projectName + ".csv";

            return new MetricsCSVExporter.Builder(outputPath)
                .Mapping(metrics.Mapping)
                .SourceCodeMetrics(metrics.SourceCodeMetrics)
                .TestCodeMetrics(metrics.TestCodeMetrics)
                .BaseMetrics(metrics.TestPathMetrics)
                .UsingDelimiter(DELIMITER)
                .Build(); ;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private static void CheckRequiredFields(string outputDir, string projectName, MetricsContainer metrics)
        {
            if ((outputDir == null) || outputDir.Length == 0)
                throw new ArgumentException("Output directory cannot be empty");

            if ((projectName == null) || projectName.Length == 0)
                throw new ArgumentException("Project name cannot be empty");

            if (metrics == null)
                throw new ArgumentException("Metrics cannot be null");
        }
    }
}
