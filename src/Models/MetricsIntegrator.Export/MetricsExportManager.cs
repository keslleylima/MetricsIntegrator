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
                                    IDictionary<string, Metrics> sourceCodeMetrics,
                                    IDictionary<string, Metrics> codeCoverage,
                                    FilterMetrics filterMetrics)
        {
            this.codeCoverage = codeCoverage;

            exportFactory = new MetricsExporterFactory.Builder()
                .OutputPath(outputPath)
                .SourceCodeMetrics(sourceCodeMetrics)
                .FilterMetrics(filterMetrics)
                .Build();
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string outputPath;
            private IDictionary<string, Metrics> sourceCodeMetrics;
            private IDictionary<string, Metrics> codeCoverage;
            private FilterMetrics filterMetrics;


            public Builder()
            {
                outputPath = default!;
                sourceCodeMetrics = default!;
                codeCoverage = default!;
                filterMetrics = default!;
            }

            public Builder OutputPath(string path)
            {
                outputPath = path;
                
                return this;
            }

            public Builder SourceCodeMetrics(IDictionary<string, Metrics> metrics)
            {
                sourceCodeMetrics = metrics;

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
                    sourceCodeMetrics,
                    codeCoverage,
                    filterMetrics
                );
            }

            private void ValidateRequiredFields()
            {
                if ((outputPath == null) || outputPath.Length == 0)
                    throw new ArgumentException("Output path cannot be empty");

                if (sourceCodeMetrics == null)
                    throw new ArgumentException("Source code metrics cannot be null");

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
