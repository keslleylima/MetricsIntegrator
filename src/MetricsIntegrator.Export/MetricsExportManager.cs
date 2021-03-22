using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Export
{
    public class MetricsExportManager : IExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly List<Metrics> testPathMetrics;
        private readonly List<Metrics> testCaseMetrics;
        private readonly MetricsExporterFactory exportFactory;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsExportManager(string outputDirectoryPath,
                                    string projectName,
                                    Dictionary<string, List<string>> mapping,
                                    Dictionary<string, Metrics> sourceCodeMetrics,
                                    Dictionary<string, Metrics> testCodeMetrics,
                                    List<Metrics> testPathMetrics,
                                    List<Metrics> testCaseMetrics)
        {
            this.testPathMetrics = testPathMetrics;
            this.testCaseMetrics = testCaseMetrics;

            exportFactory = new MetricsExporterFactory.Builder()
                .OutputDirectory(outputDirectoryPath)
                .ProjectName(projectName)
                .Mapping(mapping)
                .SourceCodeMetrics(sourceCodeMetrics)
                .TestCodeMetrics(testCodeMetrics)
                .Build();
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
            private List<Metrics> testPathMetrics;
            private List<Metrics> testCaseMetrics;

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

            public Builder TestPathMetrics(List<Metrics> metrics)
            {
                testPathMetrics = metrics;

                return this;
            }

            public Builder TestCaseMetrics(List<Metrics> metrics)
            {
                testCaseMetrics = metrics;

                return this;
            }

            public MetricsExportManager Build()
            {
                ValidateRequiredFields();

                return new MetricsExportManager(
                    outputDirectoryPath,
                    projectName,
                    mapping,
                    sourceCodeMetrics,
                    testCodeMetrics,
                    testPathMetrics,
                    testCaseMetrics
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

                if ((testPathMetrics == null) && (testCaseMetrics == null))
                    throw new ArgumentException("Test path or test case metrics must be provided");
            }
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            ExportUsingTestPathMetrics();
            ExportUsingTestCaseMetrics();
        }

        private void ExportUsingTestPathMetrics()
        {
            IExporter csvExporter = exportFactory.CreateTestPathCSVExporter(testPathMetrics);
            csvExporter.Export();
        }

        private void ExportUsingTestCaseMetrics()
        {
            IExporter csvExporter = exportFactory.CreateTestCaseCSVExporter(testCaseMetrics);
            csvExporter.Export();
        }
    }
}
