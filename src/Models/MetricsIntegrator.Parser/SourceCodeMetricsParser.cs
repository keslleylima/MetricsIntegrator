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
        private readonly IDictionary<string, List<string>> mapping;
        private string delimiter = default!;
        private int identifierColumnIndex = default!;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public SourceCodeMetricsParser(string filepath, 
                                       IDictionary<string, List<string>> mapping)
        {
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            if (mapping == null)
                throw new ArgumentException("Mapping cannot be null");

            this.filepath = filepath;
            this.mapping = mapping;

            SourceCodeMetrics = new Dictionary<string, Metrics>();
            SourceTestMetrics = new Dictionary<string, Metrics>();
            FieldKeys = new List<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        /// <summary>
        ///     Tested methods and constructors by a test method.
        /// </summary>
        public Dictionary<string, Metrics> SourceCodeMetrics { get; private set; }

        /// <summary>
        ///     Test methods that are tested by a test method.
        /// </summary>
        public Dictionary<string, Metrics> SourceTestMetrics { get; private set; }

        public List<string> FieldKeys { get; private set; }


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
                string[] columns = line.Split(delimiter);

                if (!IsMethodOrConstructorSignature(columns[identifierColumnIndex]))
                    continue;
                
                if (IsTestMethod(columns[identifierColumnIndex]))
                    ParseTestMethod(columns, fields);
                else
                    ParseTestedMethodOrConstructor(columns, fields);
            }
        }

        private bool IsMethodOrConstructorSignature(string signature)
        {
            Regex regexSignature = new Regex(
                @".*([^.]\.)+[^.]+\(.*\).*",
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            );

            return regexSignature.IsMatch(signature);
        }

        private bool IsTestMethod(string signature)
        {
            return !mapping.ContainsKey(signature);
        }

        private void ParseTestMethod(string[] columns, List<string> fields)
        {
            foreach (KeyValuePair<string, List<string>> kvp in mapping)
            {
                List<string> testMethods = kvp.Value;

                foreach (string testMethod in testMethods)
                {
                    if (testMethod == columns[identifierColumnIndex] && 
                        !SourceTestMetrics.ContainsKey(columns[identifierColumnIndex]))
                    {
                        SourceTestMetrics.Add(
                            columns[identifierColumnIndex], 
                            CreateMetricsContainer(columns, fields)
                        );
                    }
                }

            }
        }

        private void ParseTestedMethodOrConstructor(string[] columns, List<string> fields)
        {
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
