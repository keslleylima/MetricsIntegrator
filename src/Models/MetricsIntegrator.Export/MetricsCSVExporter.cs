using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    public class MetricsCSVExporter : IExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string outputPath;
        private Dictionary<string, List<string>> mapping;
        private Dictionary<string, Metrics> dictSourceCode;
        private Dictionary<string, Metrics> dictSourceTest;
        private string delimiter;
        private StringBuilder lines;
        private List<Metrics> listBaseMetrics;
        private ISet<string> filterMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private MetricsCSVExporter(string outputPath,
                                   string delimiter,
                                   Dictionary<string, List<string>> mapping,
                                   Dictionary<string, Metrics> dictSourceCode,
                                   Dictionary<string, Metrics> dictSourceTest,
                                   List<Metrics> baseMetrics,
                                   ISet<string> filterMetrics)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
            this.delimiter = delimiter;
            listBaseMetrics = baseMetrics;
            lines = new StringBuilder();
            this.filterMetrics = filterMetrics ?? new HashSet<string>();
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
            private Dictionary<string, List<string>> mapping;
            private Dictionary<string, Metrics> sourceCodeMetrics;
            private Dictionary<string, Metrics> testCodeMetrics;
            private List<Metrics> baseMetrics;
            private ISet<string> filterMetrics;

            public Builder()
            {
            }

            public Builder OutputPath(string outputPath)
            {
                this.outputPath = outputPath;

                return this;
            }

            public Builder Mapping(Dictionary<string, List<string>> mapping)
            {
                this.mapping = mapping;

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

            public Builder BaseMetrics(List<Metrics> metrics)
            {
                baseMetrics = metrics;

                return this;
            }

            public Builder UsingDelimiter(string delimiter)
            {
                this.delimiter = delimiter;

                return this;
            }

            public Builder FilterMetrics(ISet<string> filterMetrics)
            {
                this.filterMetrics = filterMetrics;

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
            public MetricsCSVExporter Build()
            {
                CheckRequiredFields();

                delimiter = delimiter ?? ";";

                return new MetricsCSVExporter(
                    outputPath,
                    delimiter,
                    mapping, 
                    sourceCodeMetrics, 
                    testCodeMetrics, 
                    baseMetrics,
                    filterMetrics
                );
            }

            private void CheckRequiredFields()
            {
                if (outputPath == null)
                    throw new ArgumentException("Output path cannot be null");

                if (mapping == null)
                    throw new ArgumentException("Mapping cannot be null");

                if (sourceCodeMetrics == null)
                    throw new ArgumentException("Source code metrics cannot be null");

                if (testCodeMetrics == null)
                    throw new ArgumentException("Test code metrics cannot be null");

                if (baseMetrics == null)
                    throw new ArgumentException("Base metrics cannot be null");
            }
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            WriteHeader();
            WriteBody();
            SaveFile();
        }

        private void WriteBody()
        {
            foreach (KeyValuePair<string, List<string>> kvp in mapping)
            {
                string testedMethod = kvp.Key;
                List<string> testMethods = kvp.Value;

                dictSourceCode.TryGetValue(testedMethod, out Metrics metricsSourceCode);

                if (metricsSourceCode == null)
                    continue;

                foreach (string testMethod in testMethods)
                {
                    dictSourceTest.TryGetValue(testMethod, out Metrics metricsSourceTest);

                    if (metricsSourceTest == null)
                        continue;

                    foreach (Metrics baseMetrics in listBaseMetrics)
                    {
                        if (!baseMetrics.GetID().Equals(testMethod))
                            continue;

                        WriteMetricsOfTestedMethod(metricsSourceCode);
                        WriteMetricsOfTestMethod(metricsSourceTest);
                        WriteMetricsOfBaseMetrics(baseMetrics);
                        WriteBreakLine();
                    }
                }
            }
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

        private string[] GetTestedMethodMetrics()
        {
            if (dictSourceTest.Count == 0)
                return new string[0];

            return GetFirstMetricFrom(dictSourceTest).GetAllMetrics();
        }

        private Metrics GetFirstMetricFrom(Dictionary<string, Metrics> dictionary)
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

        private string[] GetTestMethodMetrics()
        {
            if (dictSourceTest.Count == 0)
                return new string[0];

            return GetFirstMetricFrom(dictSourceTest).GetAllMetrics();
        }

        private void WriteBaseMetrics()
        {
            foreach (string metric in GetBaseMetrics())
            {
                lines.Append(metric);
                lines.Append(delimiter);
            }
        }

        private string[] GetBaseMetrics()
        {
            return listBaseMetrics[0].GetAllMetrics();
        }

        private void WriteMetricsOfTestedMethod(Metrics metricsSourceCode)
        {
            foreach (string metricValue in metricsSourceCode.GetAllMetricValues())
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfTestMethod(Metrics metricsSourceTest)
        {
            foreach (string metricValue in metricsSourceTest.GetAllMetricValues())
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfBaseMetrics(Metrics baseMetrics)
        {
            foreach (string metricValue in baseMetrics.GetAllMetricValues())
            {
                if (IsMetricInFilter(metricValue))
                    continue;

                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private bool IsMetricInFilter(string metric)
        {
            return filterMetrics.Contains(metric);
        }

        private void WriteBreakLine()
        {
            lines.Append("\n");
        }

        private void SaveFile()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            Directory.CreateDirectory(Directory.GetParent(outputPath).FullName);

            File.WriteAllText(outputPath, lines.ToString());
        }
    }
}
