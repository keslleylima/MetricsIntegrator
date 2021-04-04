using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetricsIntegrator.Data;

namespace MetricsIntegrator.Parser
{
    public class SourceCodeMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private readonly Dictionary<string, List<string>> mapping;
        private readonly string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public SourceCodeMetricsParser(string filepath, 
                                       Dictionary<string, List<string>> mapping,
                                       string delimiter)
        {
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File does not exist: " + filepath);

            if (mapping == null)
                throw new ArgumentException("Mapping cannot be null");

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be empty");

            this.filepath = filepath;
            this.mapping = mapping;
            this.delimiter = delimiter;

            SourceCodeMetrics = new Dictionary<string, Metrics>();
            SourceTestMetrics = new Dictionary<string, Metrics>();
        }

        public SourceCodeMetricsParser(string filepath, 
                                       Dictionary<string, List<string>> mapping)
            : this(filepath, mapping, ";")
        {
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
            FieldKeys = ExtractFieldKeys(lines);
            
            ParseMetrics(lines, FieldKeys);
        }

        private void ParseMetrics(string[] lines, List<string> fields)
        {
            foreach (string line in lines.Skip(1).ToArray())
            {
                string[] columns = line.Split(delimiter);

                if (!IsMethodOrConstructorSignature(columns[0]))
                    continue;
                
                if (IsTestMethod(columns[0]))
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
                    if (testMethod == columns[0] && !SourceTestMetrics.ContainsKey(columns[0]))
                    {
                        SourceTestMetrics.Add(columns[0], CreateMetricsContainer(columns, fields));
                    }
                }

            }
        }

        private void ParseTestedMethodOrConstructor(string[] columns, List<string> fields)
        {
            SourceCodeMetrics.Add(columns[0], CreateMetricsContainer(columns, fields));
        }

        private List<string> ExtractFieldKeys(string[] lines)
        {
            return lines[0].Split(delimiter).ToList<string>();
        }

        private Metrics CreateMetricsContainer(string[] row, List<string> fields)
        {
            Metrics metricsSourceTest = new Metrics();

            for (int i = 0; i < fields.Count; i++)
            {
                metricsSourceTest.AddMetric(fields[i], row[i]);
            }

            return metricsSourceTest;
        }
    }
}
