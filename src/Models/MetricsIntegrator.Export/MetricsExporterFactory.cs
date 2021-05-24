using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Export
{
    /// <summary>
    ///     Responsible for creating metrics exporters.
    /// </summary>
    public class MetricsExporterFactory
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly string DELIMITER = ";";
        private readonly string outputPath;
        private readonly IDictionary<string, Metrics> sourceCodeMetrics;
        private readonly FilterMetrics filterMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsExporterFactory(string outputPath,
                                       IDictionary<string, Metrics> sourceCodeMetrics,
                                       FilterMetrics filterMetrics)
        {
            this.outputPath = outputPath;
            this.sourceCodeMetrics = sourceCodeMetrics;
            this.filterMetrics = filterMetrics;
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string outputPath;
            private IDictionary<string, Metrics> sourceCodeMetrics;
            private FilterMetrics filterMetrics;

            public Builder()
            {
                outputPath = default!;
                sourceCodeMetrics = default!;
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

            public Builder FilterMetrics(FilterMetrics filterMetrics)
            {
                this.filterMetrics = filterMetrics;

                return this;
            }

            public MetricsExporterFactory Build()
            {
                ValidateRequiredFields();

                return new MetricsExporterFactory(
                    outputPath,
                    sourceCodeMetrics,
                    filterMetrics
                );
            }

            private void ValidateRequiredFields()
            {
                if ((outputPath == null) || outputPath.Length == 0)
                    throw new ArgumentException("Output directory cannot be empty");

                if (sourceCodeMetrics == null)
                    throw new ArgumentException("Source code metrics cannot be null");
            }
        }


        //---------------------------------------------------------------------
        //		Factories
        //---------------------------------------------------------------------
        public IExporter CreateCodeCoverageCSVExporter(IDictionary<string, Metrics> metrics)
        {
            if ((metrics == null) || metrics.Count == 0)
                throw new ArgumentException("There are no metrics");

            return CreateMetricsCSVExporter(
                outputPath, 
                metrics, 
                filterMetrics.CodeCoverageFilter
            );
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private IExporter CreateMetricsCSVExporter(string outputPath, 
                                                   IDictionary<string, Metrics> metrics,
                                                   ISet<string> baseMetricsFilter)
        {
            return new MetricsCsvExporter.Builder()
               .OutputPath(outputPath)
               .SourceCodeMetrics(sourceCodeMetrics)
               .CoverageMetrics(metrics)
               .UsingDelimiter(DELIMITER)
               .SourceCodeMetricsFilter(filterMetrics.SourceCodeMetricsFilter)
               .BaseMetricsFilter(baseMetricsFilter)
               .Build();
        }
    }
}
