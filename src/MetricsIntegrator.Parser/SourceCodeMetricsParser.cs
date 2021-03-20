using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetricsIntegrator.Export;
using MetricsIntegrator.Metrics;

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
        public SourceCodeMetricsParser(string filepath, Dictionary<string, List<string>> mapping,
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
        }

        public SourceCodeMetricsParser(string filepath, Dictionary<string, List<string>> mapping)
            : this(filepath, mapping, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, MetricsContainer> DictSourceCode { get; private set; }
        public Dictionary<string, MetricsContainer> DictSourceTest { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Parse()
        {
            DictSourceCode = new Dictionary<string, MetricsContainer>();
            DictSourceTest = new Dictionary<string, MetricsContainer>();
            string[] sourceMetricsFile = File.ReadAllLines(filepath);
            string[] fields = sourceMetricsFile[0].Split(delimiter);

            foreach (string line in sourceMetricsFile.Skip(1).ToArray())
            {
                string[] column;
                column = line.Split(delimiter);
                if (mapping.ContainsKey(column[0])) // column[0]: Name  }-> if (current method is a tested method)
                {
                    DictSourceCode.Add(column[0], CreateMetricsContainer(column, fields));
                }
                else // else current method is a test method
                {
                    foreach (KeyValuePair<string, List<string>> kvp in mapping)
                    {
                        List<string> keysTest = kvp.Value;
                        foreach (string key in keysTest)
                        {
                            if (key == column[0] && !DictSourceTest.ContainsKey(column[0]))
                            {
                                DictSourceTest.Add(column[0], CreateMetricsContainer(column, fields));
                            }
                        }

                    }

                }
            }
        }

        private MetricsContainer CreateMetricsContainer(string[] row, string[] fields)
        {
            MetricsContainer metricsSourceTest = new MetricsContainer();

            for (int i = 1; i < fields.Length; i++)
            {
                metricsSourceTest.AddMetric(fields[i], row[i]);
            }

            return metricsSourceTest;
        }
    }
}
