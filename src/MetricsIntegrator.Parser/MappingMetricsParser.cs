using System;
using System.Collections.Generic;
using System.IO;

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
            if ((filepath == null) || filepath.Length == 0)
                throw new ArgumentException("File path cannot be empty");

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be empty");

            if (!File.Exists(filepath))
                throw new ArgumentException("File path does not exist: " + filepath);

            this.filepath = filepath;
            this.delimiter = delimiter;
        }

        public MappingMetricsParser(string filepath) : this(filepath, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        /// <summary>
        ///     Analyzes the file and converts its information into a dictionary
        ///     containing tested invoked + test methods that test it.
        /// </summary>
        /// 
        /// <returns>
        ///     Dictionary whose keys are the tested methods and whose values
        ///     are the test methods that test it.
        /// </returns>
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
