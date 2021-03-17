using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class SourceCodeMetricsParser
    {
        private readonly string filepath;
        
        public SourceCodeMetricsParser(string filepath)
        {
            this.filepath = filepath;
        }

        public Dictionary<string, SourceCodeMetrics> DictSourceCode { get; private set; }
        public Dictionary<string, SourceTestMetrics> DictSourceTest { get; private set; }

        public void Parse()
        {
            DictSourceCode = new Dictionary<string, SourceCodeMetrics>();
            DictSourceTest = new Dictionary<string, SourceTestMetrics>();
            string[] sourceMetricsFile = File.ReadAllLines(filepath);
            foreach (string line in sourceMetricsFile.Skip(1).ToArray())
            {
                string[] column;
                column = line.Split(";");
                if (mapping.ContainsKey(column[1]))
                {
                    SourceCodeMetrics metricsSourceCode = new SourceCodeMetrics();
                    SetSourceCodeMetrics(metricsSourceCode, column);
                    DictSourceCode.Add(column[1], metricsSourceCode);
                }
                else
                {
                    foreach (KeyValuePair<string, string[]> kvp in mapping)
                    {
                        string[] keysTest = kvp.Value;
                        foreach (string key in keysTest)
                        {
                            if (key == column[1])
                            {
                                SourceTestMetrics metricsSourceTest = new SourceTestMetrics();
                                SetSourceTestMetrics(metricsSourceTest, column);
                                DictSourceTest.Add(column[1], metricsSourceTest);

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
        private void SetSourceTestMetrics(SourceTestMetrics mst, string[] row)
        {
            mst.countInput = Int32.Parse(row[2]);
            mst.countLineCode = Int32.Parse(row[3]);
            mst.countLineCodeDecl = Int32.Parse(row[4]);
            mst.countLineCodeExe = Int32.Parse(row[5]);
            mst.countOutput = Int32.Parse(row[6]);
            mst.countPath = Int32.Parse(row[7]);
            mst.countPathLog = Int32.Parse(row[8]);
            mst.countStmt = Int32.Parse(row[9]);
            mst.countStmtDecl = Int32.Parse(row[10]);
            mst.countStmtExe = Int32.Parse(row[11]);
            mst.cyclomatic = Int32.Parse(row[12]);
            mst.cyclomaticModified = Int32.Parse(row[13]);
            mst.cyclomaticStrict = Int32.Parse(row[14]);
            mst.essential = Int32.Parse(row[15]);
            mst.knots = Int32.Parse(row[16]);
            mst.maxEssentialKnots = Int32.Parse(row[17]);
            mst.maxNesting = Int32.Parse(row[18]);
            mst.minEssentialKnots = Int32.Parse(row[19]);
        }
    }
}
