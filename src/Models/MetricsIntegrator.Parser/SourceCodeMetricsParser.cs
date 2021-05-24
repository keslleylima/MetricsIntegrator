using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetricsIntegrator.Data;

namespace MetricsIntegrator.Parser
{
    /// <summary>
    ///     Responsible for parsing source code metrics.
    /// </summary>
    public class SourceCodeMetricsParser
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
        public SourceCodeMetricsParser(string filepath)
        {
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            this.filepath = filepath;

            SourceCodeMetrics = new Dictionary<string, Metrics>();
            FieldKeys = new List<string>();
            SourceCodeIdentifierKey = default!;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        /// <summary>
        ///     Tested methods and constructors by a test method.
        /// </summary>
        public Dictionary<string, Metrics> SourceCodeMetrics { get; private set; }

        public List<string> FieldKeys { get; private set; }
        public string SourceCodeIdentifierKey { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Parse()
        {
            string[] lines = File.ReadAllLines(filepath);
            
            ParseHeader(lines[0]);
            ParseMetrics(lines, FieldKeys);
        }

        private void ParseHeader(string header)
        {
            delimiter = ExtractDelimiterFrom(header);
            FieldKeys = ExtractFieldKeysFrom(header, delimiter);
            identifierColumnIndex = ExtractIdentifierColumnIndexFrom(FieldKeys);

            if (identifierColumnIndex == -1)
                throw new ApplicationException("Identifier column not found");

            SourceCodeIdentifierKey = FieldKeys[identifierColumnIndex];
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
                "(id|identifier|name|idx|index|signature)",
                RegexOptions.IgnoreCase
            );

            for (int i = 0; i < fieldKeys.Count; i++)
            {
                if (identifier.IsMatch(fieldKeys[i].Trim()))
                    return i;
            }

            return -1;
        }

        private void ParseMetrics(string[] lines, List<string> fields)
        {
            foreach (string line in lines.Skip(1).ToArray())
            {
                if (!line.Contains(")") || !line.Contains("("))
                    continue;

                ParseMethod(ExtractTermsOf(line), fields);
            }
        }

        private string[] ExtractTermsOf(string line)
        {
            string[] terms;
            string normalizedLine = line.Replace("\"", "");
            string parameterSeparator = "^";
            int parenthesesStart = normalizedLine.IndexOf("(");
            int parenthesesEnd = normalizedLine.IndexOf(")");
            string parenthesesContent = normalizedLine.Substring(parenthesesStart + 1, parenthesesEnd - parenthesesStart - 1);

            string lineWithNormalizedParametersSeparator = normalizedLine.Substring(0, parenthesesStart + 1)
                + parenthesesContent.Replace(",", parameterSeparator)
                + normalizedLine.Substring(parenthesesEnd);

            terms = lineWithNormalizedParametersSeparator.Split(delimiter);

            for (int i = 0; i < terms.Length; i++)
            {
                terms[i] = terms[i].Replace(parameterSeparator, ",");
            }

            return terms;
        }

        private void ParseMethod(string[] columns, List<string> fields)
        {
            if (SourceCodeMetrics.ContainsKey(columns[identifierColumnIndex]))
                return;

            SourceCodeMetrics.Add(
                columns[identifierColumnIndex],
                CreateMetricsContainer(columns, fields)
            );
        }

        private Metrics CreateMetricsContainer(string[] columns, List<string> fields)
        {
            Metrics metricsSourceTest = new Metrics(fields[identifierColumnIndex]);

            for (int i = 0; i < fields.Count; i++)
            {
                metricsSourceTest.AddMetric(fields[i], columns[i]);
            }

            return metricsSourceTest;
        }
    }
}
