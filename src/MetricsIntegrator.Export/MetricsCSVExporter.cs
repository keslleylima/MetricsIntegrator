using MetricsIntegrator.Data;
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
        private Dictionary<string, Metrics> dictSourceCode;
        private Dictionary<string, Metrics> dictSourceTest;
        private string delimiter;
        private StringBuilder lines;
        private List<Metrics> listBaseMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsCSVExporter(string outputPath,
                                    Dictionary<string, List<string>> mapping,
                                    Dictionary<string, Metrics> dictSourceCode,
                                    Dictionary<string, Metrics> dictSourceTest,
                                    List<Metrics> baseMetrics,
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

                dictSourceCode.TryGetValue(testedMethod, out Metrics metricsSourceCode);

                foreach (string testMethod in testMethods)
                {
                    dictSourceTest.TryGetValue(testMethod, out Metrics metricsSourceTest);

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
