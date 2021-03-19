using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Parser
{
    /// <summary>
    ///     Mapping containing tested methods along with the test methods that
    ///     test them.
    /// </summary>
    public class MappingMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;
        private readonly string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MappingMetricsParser(string filepath, string delimiter)
        {
            this.filepath = filepath;
            this.delimiter = delimiter;
        }

        public MappingMetricsParser(string filepath) : this(filepath, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public Dictionary<string, List<string>> Parse()
        {
            Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>();

            foreach (string line in File.ReadAllLines(filepath))
            {
                if (IsBlank(line))
                    continue;

                string[] columns = line.Split(delimiter);
                string testedMethod = columns[0];
                List<string> testMethods = ExtractTestMethods(columns);

                mapping.Add(testedMethod, testMethods);
            }

            return mapping;
        }

        private bool IsBlank(string line)
        {
            return line.Trim().Equals("");
        }

        private List<string> ExtractTestMethods(string[] columns)
        {
            List<string> testMethods = new List<string>();

            foreach (string column in columns[1..columns.Length])
            {
                if (column.Length == 0)
                    continue;

                testMethods.Add(column);
            }

            return testMethods;
        }
    }
}
