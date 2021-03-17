using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetricsIntegrator.Export;
using MetricsIntegrator.Metric;

namespace MetricsIntegrator.Parser
{
    class SourceCodeMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string filepath;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public SourceCodeMetricsParser(string filepath)
        {
            this.filepath = filepath;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, SourceCodeMetrics> DictSourceCode { get; private set; }
        public Dictionary<string, Metrics> DictSourceTest { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Parse()
        {
            DictSourceCode = new Dictionary<string, SourceCodeMetrics>();
            DictSourceTest = new Dictionary<string, Metrics>();
            string[] sourceMetricsFile = File.ReadAllLines(filepath);
            string[] fields = sourceMetricsFile[0].Split(";");

            foreach (string line in sourceMetricsFile.Skip(1).ToArray())
            {
                string[] column;
                column = line.Split(";");
                if (mapping.ContainsKey(column[0])) // column[1]: Name  }-> if (current method is a tested method)
                {
                    SourceCodeMetrics metricsSourceCode = new SourceCodeMetrics();
                    SetSourceCodeMetrics(metricsSourceCode, column);
                    DictSourceCode.Add(column[0], metricsSourceCode);
                }
                else // else current method is a test method
                {
                    foreach (KeyValuePair<string, string[]> kvp in mapping)
                    {
                        string[] keysTest = kvp.Value;
                        foreach (string key in keysTest)
                        {
                            if (key == column[0])
                            {
                                DictSourceTest.Add(column[0], SetSourceTestMetrics(column, fields));
                            }
                        }

                    }

                }
            }
        }

        private void SetSourceCodeMetrics(SourceCodeMetrics msc, string[] row)
        {
            msc.countInput = Int32.Parse(row[2]);
            msc.countLineCode = Int32.Parse(row[3]);
            msc.countLineCodeDecl = Int32.Parse(row[4]);
            msc.countLineCodeExe = Int32.Parse(row[5]);
            msc.countOutput = Int32.Parse(row[6]);
            msc.countPath = Int32.Parse(row[7]);
            msc.countPathLog = Int32.Parse(row[8]);
            msc.countStmt = Int32.Parse(row[9]);
            msc.countStmtDecl = Int32.Parse(row[10]);
            msc.countStmtExe = Int32.Parse(row[11]);
            msc.cyclomatic = Int32.Parse(row[12]);
            msc.cyclomaticModified = Int32.Parse(row[13]);
            msc.cyclomaticStrict = Int32.Parse(row[14]);
            msc.essential = Int32.Parse(row[15]);
            msc.knots = Int32.Parse(row[16]);
            msc.maxEssentialKnots = Int32.Parse(row[17]);
            msc.maxNesting = Int32.Parse(row[18]);
            msc.minEssentialKnots = Int32.Parse(row[19]);
        }
        private Metrics SetSourceTestMetrics(string[] row, string[] fields)
        {
            Metrics metricsSourceTest = new Metrics();

            for (int i = 0; i < fields.Length; i++)
            {
                metricsSourceTest.AddMetric(fields[i], row[i]);
            }

            return metricsSourceTest;
        }
    }
}
