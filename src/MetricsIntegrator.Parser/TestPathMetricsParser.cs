using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    public class TestPathMetricsParser
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
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be empty");

            this.filepath = filepath;
            this.delimiter = delimiter;
        }

        public TestPathMetricsParser(string filepath) : this(filepath, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<Metrics> Parse()
        {
            List<Metrics> metrics = new List<Metrics>();

            string[] testPathMetricsFile = File.ReadAllLines(filepath);
            string[] fields = testPathMetricsFile[0].Split(delimiter);

            foreach (string line in testPathMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(CreateTestPathMetrics(line.Split(delimiter), fields));
            }

            return metrics;
        }

        private Metrics CreateTestPathMetrics(string[] row, string[] fields)
        {
            Metrics testPath = new Metrics();

            for (int i = 0; i < fields.Length; i++)
            {
                testPath.AddMetric(fields[i], row[i]);
            }

            return testPath;
        }
    }
}
