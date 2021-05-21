using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    /// <summary>
    ///     Responsible for exporting metrics to a CSV file.
    /// </summary>
    public class MetricsCsvExporter : IExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string outputPath;
        private readonly IDictionary<string, List<string>> mapping;
        private readonly IDictionary<string, Metrics> sourceCodeMetrics;
        private readonly IDictionary<string, Metrics> testCodeMetrics;
        private readonly string delimiter;
        private readonly StringBuilder lines;
        private readonly ISet<string> sourceCodeMetricsFilter;
        private readonly ISet<string> baseMetricsFilter;
        private readonly IDictionary<string, Metrics> coverageMetrics; 


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsCsvExporter(string outputPath,
                                   string delimiter,
                                   IDictionary<string, List<string>> mapping,
                                   IDictionary<string, Metrics> dictSourceCode,
                                   IDictionary<string, Metrics> dictSourceTest,
                                   IDictionary<string, Metrics> coverageMetrics,
                                   ISet<string> sourceCodeMetricsFilter,
                                   ISet<string> baseMetricsFilter)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.sourceCodeMetrics = dictSourceCode;
            this.testCodeMetrics = dictSourceTest;
            this.delimiter = delimiter;
            this.coverageMetrics = coverageMetrics;
            lines = new StringBuilder();
            this.sourceCodeMetricsFilter = sourceCodeMetricsFilter;
            this.baseMetricsFilter = baseMetricsFilter;
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        /// <summary>
        ///     Creates an instance of MetricsCSVExporter. It is mandatory to
        ///     provide the following information:
        ///     <list type="bullet">
        ///         <item>Output path</item>
        ///         <item>Mapping</item>
        ///         <item>Source code metrics</item>
        ///         <item>Test code metrics</item>
        ///         <item>Base metrics</item>
        ///     </list>
        /// </summary>
        public class Builder
        {
            private string outputPath;
            private string delimiter;
            private IDictionary<string, List<string>> mapping;
            private IDictionary<string, Metrics> sourceCodeMetrics;
            private IDictionary<string, Metrics> testCodeMetrics;
            private IDictionary<string, Metrics> coverageMetrics;
            private ISet<string> sourceCodeMetricsFilter;
            private ISet<string> baseMetricsFilter;

            public Builder()
            {
                outputPath = default!;
                delimiter = default!;
                mapping = default!;
                sourceCodeMetrics = default!;
                testCodeMetrics = default!;
                coverageMetrics = default!;
                sourceCodeMetricsFilter = default!;
                baseMetricsFilter = default!;
            }

            public Builder OutputPath(string outputPath)
            {
                this.outputPath = outputPath;

                return this;
            }

            public Builder Mapping(IDictionary<string, List<string>> mapping)
            {
                this.mapping = mapping;

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

            public Builder CoverageMetrics(IDictionary<string, Metrics> metrics)
            {
                coverageMetrics = metrics;

                return this;
            }

            public Builder UsingDelimiter(string delimiter)
            {
                this.delimiter = delimiter;

                return this;
            }

            public Builder SourceCodeMetricsFilter(ISet<string> sourceCodeMetricsFilter)
            {
                this.sourceCodeMetricsFilter = sourceCodeMetricsFilter;

                return this;
            }

            public Builder BaseMetricsFilter(ISet<string> baseMetricsFilter)
            {
                this.baseMetricsFilter = baseMetricsFilter;

                return this;
            }

            /// <summary>
            ///     Creates an instance of MetricsCSVExporter.
            /// </summary>
            /// 
            /// <returns>
            ///     MetricsCSVExporter
            /// </returns>
            /// 
            /// <exception cref="System.ArgumentException">
            ///     If any required field has not been provided.
            /// </exception>
            public MetricsCsvExporter Build()
            {
                CheckRequiredFields();

                return new MetricsCsvExporter(
                    outputPath,
                    delimiter,
                    mapping, 
                    sourceCodeMetrics, 
                    testCodeMetrics,
                    coverageMetrics,
                    sourceCodeMetricsFilter,
                    baseMetricsFilter
                );
            }

            private void CheckRequiredFields()
            {
                if (outputPath == null)
                    throw new ArgumentException("Output path cannot be null");

                if ((mapping == null) || (mapping.Count == 0))
                    throw new ArgumentException("Mapping cannot be empty");

                if ((sourceCodeMetrics == null) || (sourceCodeMetrics.Count == 0))
                    throw new ArgumentException("Source code metrics cannot be empty");

                if ((testCodeMetrics == null) || (testCodeMetrics.Count == 0))
                    throw new ArgumentException("Test code metrics cannot be empty");

                if ((coverageMetrics == null) || (coverageMetrics.Count == 0))
                    throw new ArgumentException("Coverage metrics cannot be empty");
            }
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            WriteHeader();
            WriteBreakLine();
            WriteBody();
            WriteBreakLine();
            SaveFile();
        }

        private void WriteHeader()
        {
            WriteTestedMethodMetrics();
            WriteTestMethodMetrics();
            WriteBaseMetrics();
        }

        private void WriteTestedMethodMetrics()
        {
            foreach (string metric in GetTestedMethodMetrics())
            {
                lines.Append(metric);
                lines.Append(delimiter);
            }
        }

        private List<string> GetTestedMethodMetrics()
        {
            return GetFirstMetricFrom(testCodeMetrics).GetAllMetrics(sourceCodeMetricsFilter);
        }

        private Metrics GetFirstMetricFrom(IDictionary<string, Metrics> dictionary)
        {
            var dictEnum = dictionary.GetEnumerator();
            dictEnum.MoveNext();

            return dictEnum.Current.Value;
        }

        private void WriteTestMethodMetrics()
        {
            foreach (string metric in GetTestMethodMetrics())
            {
                lines.Append(metric);
                lines.Append(delimiter);
            }
        }

        private List<string> GetTestMethodMetrics()
        {
            if (testCodeMetrics.Count == 0)
                return new List<string>();

            return GetFirstMetricFrom(testCodeMetrics).GetAllMetrics(sourceCodeMetricsFilter);
        }

        private void WriteBaseMetrics()
        {
            foreach (string metric in GetBaseMetrics())
            {
                lines.Append(metric);
                lines.Append(delimiter);
            }
        }

        private List<string> GetBaseMetrics()
        {
            var firstOfBaseMetrics = coverageMetrics.GetEnumerator();
            firstOfBaseMetrics.MoveNext();

            var firstMetric = firstOfBaseMetrics.Current.Value;
   
            return firstMetric.GetAllMetrics(baseMetricsFilter);
        }

        private void WriteBody()
        {
            foreach (KeyValuePair<string, List<string>> kvp in mapping)
            {
                string testedMethod = kvp.Key;
                List<string> testMethods = kvp.Value;

                if (!sourceCodeMetrics.ContainsKey(testedMethod))
                    continue;

                WriteMetricsOfTestedMethod(testedMethod, testMethods);
            }
        }

        private void WriteMetricsOfTestedMethod(string testedMethod, List<string> testMethods)
        {
            foreach (string testMethod in testMethods)
            {
                if (!testCodeMetrics.ContainsKey(testMethod))
                    continue;

                if (!coverageMetrics.ContainsKey(testMethod + testedMethod))
                    continue;

                coverageMetrics.TryGetValue(
                    testMethod + testedMethod, 
                    out Metrics? metrics
                );

                if (metrics != null)
                {
                    WriteMetricsOfTestedMethod(testedMethod);
                    WriteMetricsOfTestMethod(testMethod);
                    WriteMetricsOfBaseMetrics(metrics);
                    WriteBreakLine();
                }
            }
        }

        private void WriteMetricsOfTestedMethod(string testedMethod)
        {
            sourceCodeMetrics.TryGetValue(testedMethod, out Metrics? metricsSourceCode);

            List<string> metricValues = metricsSourceCode
                ?.GetAllMetricValues(sourceCodeMetricsFilter) 
                ?? new List<string>();

            foreach (string metricValue in metricValues)
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfTestMethod(string testMethod)
        {
            testCodeMetrics.TryGetValue(testMethod, out Metrics? metricsSourceTest);

            List<string>? metricValues = metricsSourceTest
                    ?.GetAllMetricValues(sourceCodeMetricsFilter) 
                    ?? new List<string>();

            foreach (string metricValue in metricValues)
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfBaseMetrics(Metrics metrics)
        {
            foreach (string metricValue in metrics.GetAllMetricValues(baseMetricsFilter))
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteBreakLine()
        {
            lines.Append("\n");
        }

        private void SaveFile()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            string? path = Directory.GetParent(outputPath)?.FullName;

            Directory.CreateDirectory(path ?? "");

            File.WriteAllText(outputPath, lines.ToString());
        }
    }
}
