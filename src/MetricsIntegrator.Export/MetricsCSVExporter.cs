using MetricsIntegrator.Metrics;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    public class MetricsCSVExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string outputPath;
        private Dictionary<string, List<string>> mapping;
        private Dictionary<string, MetricsContainer> dictSourceCode;
        private Dictionary<string, MetricsContainer> dictSourceTest;
        private string delimiter;
        private StringBuilder lines;
        private List<MetricsContainer> listBaseMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsCSVExporter(string outputPath,
                                    Dictionary<string, List<string>> mapping,
                                    Dictionary<string, MetricsContainer> dictSourceCode,
                                    Dictionary<string, MetricsContainer> dictSourceTest,
                                    List<MetricsContainer> baseMetrics,
                                    string delimiter)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
            this.delimiter = delimiter;
            listBaseMetrics = baseMetrics;
            lines = new StringBuilder();
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

                dictSourceCode.TryGetValue(testedMethod, out MetricsContainer metricsSourceCode);

                foreach (string testMethod in testMethods)
                {
                    dictSourceTest.TryGetValue(testMethod, out MetricsContainer metricsSourceTest);

                    foreach (MetricsContainer baseMetrics in listBaseMetrics)
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
            return GetFirstMetricFrom(dictSourceTest).GetAllMetrics();
        }

        private MetricsContainer GetFirstMetricFrom(Dictionary<string, MetricsContainer> dictionary)
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

        private void WriteMetricsOfTestedMethod(MetricsContainer metricsSourceCode)
        {
            foreach (string metricValue in metricsSourceCode.GetAllMetricValues())
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfTestMethod(MetricsContainer metricsSourceTest)
        {
            foreach (string metricValue in metricsSourceTest.GetAllMetricValues())
            {
                lines.Append(metricValue);
                lines.Append(delimiter);
            }
        }

        private void WriteMetricsOfBaseMetrics(MetricsContainer baseMetrics)
        {
            foreach (string metricValue in baseMetrics.GetAllMetricValues())
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

            File.WriteAllText(outputPath, lines.ToString());
        }
    }
}
