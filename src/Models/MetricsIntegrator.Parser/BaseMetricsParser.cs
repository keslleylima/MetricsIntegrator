using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    public class BaseMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private readonly string delimiter;
        private readonly ISet<string> filterMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public BaseMetricsParser(string filepath, string delimiter)
        {
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be empty");

            this.filepath = filepath;
            this.delimiter = delimiter;

            FieldKeys = new List<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<string> FieldKeys { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<Metrics> Parse()
        {
            List<Metrics> metrics = new List<Metrics>();

            string[] testPathMetricsFile = File.ReadAllLines(filepath);
            string[] fieldKey = testPathMetricsFile[0].Split(delimiter);

            StoreFieldKeys(fieldKey);

            foreach (string line in testPathMetricsFile.Skip(1).ToArray())
            {
                metrics.Add(CreateBaseMetrics(line.Split(delimiter), fieldKey));
            }

            return metrics;
        }

        private void StoreFieldKeys(string[] fieldKey)
        {
            for (int i = 0; i < fieldKey.Length; i++)
            {
                FieldKeys.Add(fieldKey[i]);
            }
        }

        private Metrics CreateBaseMetrics(string[] fieldValue, string[] fieldKey)
        {
            Metrics metrics = new Metrics();

            for (int i = 0; i < fieldKey.Length; i++)
            {
                metrics.AddMetric(fieldKey[i], fieldValue[i]);
            }

            return metrics;
        }
    }
}
