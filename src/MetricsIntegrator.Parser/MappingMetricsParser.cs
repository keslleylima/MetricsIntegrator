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


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MappingMetricsParser(string filepath)
        {
            this.filepath = filepath;
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
                column = line.Split(";");
                string testedMethod = column[0];
                string[] testMethods = column[1..column.Length];
                
                mapping.Add(testedMethod, testMethods);
            }

            return mapping;
        }
    }
}
