using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class TestCaseMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string filepath;
        private string delimiter;
        private string[] fields;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParser(string filepath, string delimiter, string[] fields)
        {
            this.filepath = filepath;
            this.delimiter = delimiter;
            this.fields = fields;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<TestCaseMetrics> Parse()
        {
            List<TestCaseMetrics> metrics = new List<TestCaseMetrics>();

            string[] testCaseMetricsFile = File.ReadAllLines(filepath);
            foreach (string line in testCaseMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(SetTestCaseMetrics(line.Split(delimiter)));
            }

            return metrics;
        }

        private TestCaseMetrics SetTestCaseMetrics(string[] row)
        {
            TestCaseMetrics testCase = new TestCaseMetrics();

            for (int i = 0; i < fields.Length; i++)
            {
                testCase.AddMetric(fields[i], row[i]);
            }

            return testCase;
        }
    }
}
