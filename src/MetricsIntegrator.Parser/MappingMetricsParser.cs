using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class MappingMetricsParser
    {
        private readonly string filepath;

        public MappingMetricsParser(string filepath)
        {
            this.filepath = filepath;
        }

        public Dictionary<string, string[]> Parse()
        {
            Dictionary<string, string[]> mapping = new Dictionary<string, string[]>();

            foreach (string line in File.ReadAllLines(filepath))
            {
                string[] column;
                column = line.Split(";");
                mapping.Add(column[0], column[1..column.Length]);
            }

            return mapping;
        }
    }
}
