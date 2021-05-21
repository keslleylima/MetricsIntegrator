using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Export
{
    /// <summary>
    ///     Responsible for handling all data exports.
    /// </summary>
    public class MetricsExportManager : IExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly IDictionary<string, Metrics> codeCoverage;
        private readonly MetricsExporterFactory exportFactory;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsExportManager(string outputPath,
                                    IDictionary<string, List<string>> mapping,
                                    IDictionary<string, Metrics> sourceCodeMetrics,
                                    IDictionary<string, Metrics> testCodeMetrics,
                                    IDictionary<string, Metrics> codeCoverage,
                                    FilterMetrics filterMetrics)
        {
            this.codeCoverage = codeCoverage;

            exportFactory = new MetricsExporterFactory.Builder()
                .OutputPath(outputPath)
                .Mapping(mapping)
                .SourceCodeMetrics(sourceCodeMetrics)
                .TestCodeMetrics(testCodeMetrics)
                .FilterMetrics(filterMetrics)
                .Build();
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string outputPath;
            private IDictionary<string, List<string>> mapping;
            private IDictionary<string, Metrics> sourceCodeMetrics;
            private IDictionary<string, Metrics> testCodeMetrics;
            private IDictionary<string, Metrics> codeCoverage;
            private FilterMetrics filterMetrics;


            public Builder()
            {
                outputPath = default!;
                mapping = default!;
                sourceCodeMetrics = default!;
                testCodeMetrics = default!;
                codeCoverage = default!;
                filterMetrics = default!;
            }

            public Builder OutputPath(string path)
            {
                outputPath = path;
                
                return this;
            }

            public Builder Mapping(IDictionary<string, List<string>> map)
            {
                mapping = map;

                return this;
            }

            public Builder SourceCodeMetrics(IDictionary<string, Metrics> metrics)
            {
                sourceCodeMetrics = metrics;

                return this;
            }

            public Builder TestCodeMetrics(IDictionary<string, Metrics> metrics)
            {
                testCodeMetrics = metrics;

                return this;
            }

            public Builder CodeCoverage(IDictionary<string, Metrics> metrics)
            {
                codeCoverage = metrics;

                return this;
            }

            public Builder FilterMetrics(FilterMetrics filterMetrics)
            {
                this.filterMetrics = filterMetrics;

                return this;
            }

            public MetricsExportManager Build()
            {
                ValidateRequiredFields();

                return new MetricsExportManager(
                    outputPath,
                    mapping,
                    sourceCodeMetrics,
                    testCodeMetrics,
                    codeCoverage,
                    filterMetrics
                );
            }

            private void ValidateRequiredFields()
            {
                if ((outputPath == null) || outputPath.Length == 0)
                    throw new ArgumentException("Output path cannot be empty");

                if (mapping == null)
                    throw new ArgumentException("Mapping cannot be null");

                if (sourceCodeMetrics == null)
                    throw new ArgumentException("Source code metrics cannot be null");

                if (testCodeMetrics == null)
                    throw new ArgumentException("Test code metrics cannot be null");

                if (codeCoverage == null)
                    throw new ArgumentException("Code coverage metrics cannot be null");
            }
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            ExportUsingCodeCoverage();
        }

        private void ExportUsingCodeCoverage()
        {
            IExporter csvExporter = exportFactory.CreateCodeCoverageCSVExporter(codeCoverage);
            csvExporter.Export();
        }
    }
}
