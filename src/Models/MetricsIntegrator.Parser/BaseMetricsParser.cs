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
            string[] lines = File.ReadAllLines(filepath);

            StoreFieldKeys(lines);
            
            return ParseMetrics(lines, FieldKeys);
        }

        private List<Metrics> ParseMetrics(string[] lines, List<string> fieldKeys)
        {
            List<Metrics> metrics = new List<Metrics>();

            foreach (string line in lines.Skip(1).ToArray())
            {
                metrics.Add(CreateBaseMetrics(line.Split(delimiter), fieldKeys));
            }

            return metrics;
        }

        private void StoreFieldKeys(string[] lines)
        {
            foreach (string field in lines[0].Split(delimiter))
            {
                FieldKeys.Add(field);
            }
        }

        private Metrics CreateBaseMetrics(string[] fieldValue, List<string> fieldKeys)
        {
            Metrics metrics = new Metrics();

            for (int i = 0; i < fieldKeys.Count; i++)
            {
                metrics.AddMetric(fieldKeys[i], fieldValue[i]);
            }

            return metrics;
        }
    }
}
