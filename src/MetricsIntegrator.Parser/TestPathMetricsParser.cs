using MetricsIntegrator.Metrics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    class TestPathMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private readonly string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestPathMetricsParser(string filepath, string delimiter)
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

            string[] testPathMetricsFile = File.ReadAllLines(filepath);
            string[] fields = testPathMetricsFile[0].Split(delimiter);

            foreach (string line in testPathMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(CreateTestPathMetrics(line.Split(delimiter), fields));
            }

            return metrics;
        }

        private MetricsContainer CreateTestPathMetrics(string[] row, string[] fields)
        {
            MetricsContainer testPath = new MetricsContainer();

            for (int i = 0; i < fields.Length; i++)
            {
                testPath.AddMetric(fields[i], row[i]);
            }

            return testPath;
        }
    }
}
