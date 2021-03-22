using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Export
{
    public class MetricsExporterFactory
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly string DELIMITER = ";";
        private readonly string outputDirectoryPath;
        private readonly string projectName;
        private readonly Dictionary<string, List<string>> mapping;
        private readonly Dictionary<string, Metrics> sourceCodeMetrics;
        private readonly Dictionary<string, Metrics> testCodeMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsExporterFactory(string outputDirectoryPath,
                                       string projectName,
                                       Dictionary<string, List<string>> mapping,
                                       Dictionary<string, Metrics> sourceCodeMetrics,
                                       Dictionary<string, Metrics> testCodeMetrics)
        {
            this.outputDirectoryPath = outputDirectoryPath;
            this.projectName = projectName;
            this.mapping = mapping;
            this.sourceCodeMetrics = sourceCodeMetrics;
            this.testCodeMetrics = testCodeMetrics;
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string outputDirectoryPath;
            private string projectName;
            private Dictionary<string, List<string>> mapping;
            private Dictionary<string, Metrics> sourceCodeMetrics;
            private Dictionary<string, Metrics> testCodeMetrics;

            public Builder()
            {
            }

            public Builder OutputDirectory(string path)
            {
                outputDirectoryPath = path;

                return this;
            }

            public Builder ProjectName(string name)
            {
                projectName = name;

                return this;
            }

            public Builder Mapping(Dictionary<string, List<string>> map)
            {
                mapping = map;

                return this;
            }

            public Builder SourceCodeMetrics(Dictionary<string, Metrics> metrics)
            {
                sourceCodeMetrics = metrics;

                return this;
            }

            public Builder TestCodeMetrics(Dictionary<string, Metrics> metrics)
            {
                testCodeMetrics = metrics;

                return this;
            }

            public MetricsExporterFactory Build()
            {
                ValidateRequiredFields();

                return new MetricsExporterFactory(
                    outputDirectoryPath,
                    projectName,
                    mapping,
                    sourceCodeMetrics,
                    testCodeMetrics
                );
            }

            private void ValidateRequiredFields()
            {
                if ((outputDirectoryPath == null) || outputDirectoryPath.Length == 0)
                    throw new ArgumentException("Output directory cannot be empty");

                if ((projectName == null) || projectName.Length == 0)
                    throw new ArgumentException("Project name cannot be empty");

                if (mapping == null)
                    throw new ArgumentException("Mapping cannot be null");

                if (sourceCodeMetrics == null)
                    throw new ArgumentException("Source code metrics cannot be null");

                if (testCodeMetrics == null)
                    throw new ArgumentException("Test code metrics cannot be null");
            }
        }


        //---------------------------------------------------------------------
        //		Factories
        //---------------------------------------------------------------------
        public IExporter CreateTestCaseCSVExporter(List<Metrics> metrics)
        {
            if ((metrics == null) || metrics.Count == 0)
                throw new ArgumentException("There are no test case metrics");

            string outputPath = outputDirectoryPath + @"\TC_dataset_resulting_" + projectName + ".csv";

            return CreateMetricsCSVExporter(outputPath, metrics);
        }

        public IExporter CreateTestPathCSVExporter(List<Metrics> metrics)
        {
            if ((metrics == null) || metrics.Count == 0)
                throw new ArgumentException("There are no test path metrics");

            string outputPath = outputDirectoryPath + @"\TP_dataset_resulting_" + projectName + ".csv";

            return CreateMetricsCSVExporter(outputPath, metrics);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private IExporter CreateMetricsCSVExporter(string outputPath, List<Metrics> metrics)
        {
            return new MetricsCSVExporter.Builder(outputPath)
               .Mapping(mapping)
               .SourceCodeMetrics(sourceCodeMetrics)
               .TestCodeMetrics(testCodeMetrics)
               .BaseMetrics(metrics)
               .UsingDelimiter(DELIMITER)
               .Build();
        }
    }
}
