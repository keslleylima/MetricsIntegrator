using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MetricsIntegrator.Parser
{
    /// <summary>
    ///     Responsible for parsing test path and test case metrics.
    /// </summary>
    public class CodeCoverageMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private string delimiter = default!;
        private int identifierColumnIndex = default!;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public CodeCoverageMetricsParser(string filepath, string delimiter)
        {
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            this.filepath = filepath;

            FieldKeys = new List<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<string> FieldKeys { get; private set; }
        public string CodeCoverageIdentifierKey { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public IDictionary<string, List<Metrics>> Parse()
        {
            string[] lines = File.ReadAllLines(filepath);

            ParseHeader(lines[0]);
            
            return ParseMetrics(lines, FieldKeys);
        }

        private void ParseHeader(string header)
        {
            delimiter = ExtractDelimiterFrom(header);
            FieldKeys = ExtractFieldKeysFrom(header, delimiter);
            identifierColumnIndex = ExtractIdentifierColumnIndexFrom(FieldKeys);

            if (identifierColumnIndex == -1)
                throw new ApplicationException("Identifier column not found");

            CodeCoverageIdentifierKey = FieldKeys[identifierColumnIndex];
        }

        private static string ExtractDelimiterFrom(string header)
        {
            Regex text = new Regex("[A-z0-9\\s\\t\\n\\r]+");

            for (int i = 0; i < header.Length; i++)
            {
                if (!text.IsMatch(char.ToString(header[i])))
                {
                    return char.ToString(header[i]);
                }
            }

            return "";
        }

        private List<string> ExtractFieldKeysFrom(string header, string delimiter)
        {
            return header.Split(delimiter).ToList<string>();
        }

        private static int ExtractIdentifierColumnIndexFrom(List<string> fieldKeys)
        {
            Regex identifier = new Regex(
                "(id|identifier|name|idx|index|signature|testmethod|tm)",
                RegexOptions.IgnoreCase
            );

            for (int i = 0; i < fieldKeys.Count; i++)
            {
                if (identifier.IsMatch(fieldKeys[i].Trim()))
                    return i;
            }

            return -1;
        }

        private IDictionary<string, List<Metrics>> ParseMetrics(string[] lines, List<string> fieldKeys)
        {
            IDictionary<string, List<Metrics>> metrics = new Dictionary<string, List<Metrics>>();

            foreach (string line in lines.Skip(1).ToArray())
            {
                Metrics metric = CreateCodeCoverageMetrics(line.Split(delimiter), fieldKeys);

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

        private Metrics CreateCodeCoverageMetrics(string[] fieldValue, List<string> fieldKeys)
        {
            Metrics metrics = new Metrics(fieldKeys[identifierColumnIndex]);

            for (int i = 0; i < fieldKeys.Count; i++)
            {
                metrics.AddMetric(fieldKeys[i], fieldValue[i]);
            }

            return metrics;
        }
    }
}
