using MetricsIntegrator.Metric;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    class TestCaseMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private readonly string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParser(string filepath, string delimiter)
        {
            this.filepath = filepath;
            this.delimiter = delimiter;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<MetricsContainer> Parse()
        {
            List<MetricsContainer> metrics = new List<MetricsContainer>();

            string[] testCaseMetricsFile = File.ReadAllLines(filepath);
            string[] fields = testCaseMetricsFile[0].Split(delimiter);

            foreach (string line in testCaseMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(CreateTestCaseMetrics(line.Split(delimiter), fields));
            }

            return metrics;
        }

        private MetricsContainer CreateTestCaseMetrics(string[] row, string[] fields)
        {
            MetricsContainer testCase = new MetricsContainer();

            for (int i = 0; i < fields.Length; i++)
            {
                testCase.AddMetric(fields[i], row[i]);
            }

            return testCase;
        }
    }
}
