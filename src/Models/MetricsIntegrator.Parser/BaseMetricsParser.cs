using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetricsIntegrator.Parser
{
    /// <summary>
    ///     Responsible for parsing test path and test case metrics.
    /// </summary>
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
        public IDictionary<string, List<Metrics>> Parse()
        {
            string[] lines = File.ReadAllLines(filepath);

            StoreFieldKeys(lines);
            
            return ParseMetrics(lines, FieldKeys);
        }

        private IDictionary<string, List<Metrics>> ParseMetrics(string[] lines, List<string> fieldKeys)
        {
            IDictionary<string, List<Metrics>> metrics = new Dictionary<string, List<Metrics>>();

            foreach (string line in lines.Skip(1).ToArray())
            {
                Metrics metric = CreateBaseMetrics(line.Split(delimiter), fieldKeys);

                if (metrics.ContainsKey(metric.GetID()))
                {
                    metrics.TryGetValue(metric.GetID(), out List<Metrics> listMetrics);
                    listMetrics.Add(metric);
                }
                else
                {
                    List<Metrics> listMetrics = new List<Metrics>();
                    listMetrics.Add(metric);

                    metrics.Add(metric.GetID(), listMetrics);
                }
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
