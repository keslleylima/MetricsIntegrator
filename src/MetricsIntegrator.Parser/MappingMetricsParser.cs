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
    class MappingMetricsParser
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
        public Dictionary<string, string[]> Parse()
        {
            Dictionary<string, string[]> mapping = new Dictionary<string, string[]>();

            foreach (string line in File.ReadAllLines(filepath))
            {
                string[] column;
                column = line.Split(delimiter);
                string testedMethod = column[0];
                string[] testMethods = column[1..column.Length];
                
                mapping.Add(testedMethod, testMethods);
            }

            return mapping;
        }
    }
}
