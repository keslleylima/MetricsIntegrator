using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    public class TestCaseMetricsParser
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
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be empty");

            this.filepath = filepath;
            this.delimiter = delimiter;
        }

        public TestCaseMetricsParser(string filepath) : this(filepath, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<Metrics> Parse()
        {
            List<Metrics> metrics = new List<Metrics>();

            string[] testCaseMetricsFile = File.ReadAllLines(filepath);
            string[] fields = testCaseMetricsFile[0].Split(delimiter);

            foreach (string line in testCaseMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(CreateTestCaseMetrics(line.Split(delimiter), fields));
            }

            return metrics;
        }

        private Metrics CreateTestCaseMetrics(string[] row, string[] fields)
        {
            Metrics testCase = new Metrics();

            for (int i = 0; i < fields.Length; i++)
            {
                testCase.AddMetric(fields[i], row[i]);
            }

            return testCase;
        }
    }
}
