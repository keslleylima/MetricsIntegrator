using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class TestPathMetricsParser
    {
        private string filepath;

        public TestPathMetricsParser(string filepath)
        {
            this.filepath = filepath;
        }

        public List<TestPathMetrics> Parse()
        {
            List<TestPathMetrics> metrics = new List<TestPathMetrics>();
            string[] testPathMetricsFile = File.ReadAllLines(filepath);
            foreach (string line in testPathMetricsFile.Skip(1).ToArray())
            {
                TestPathMetrics testPath = new TestPathMetrics();
                metrics.Add(SetTestPathMetrics(testPath, line.Split(";")));
            }

            return metrics;
        }

        private TestPathMetrics SetTestPathMetrics(TestPathMetrics tpm, string[] row)
        {
            tpm.id = row[0];
            tpm.testPath = row[1];
            tpm.pathLength = Int32.Parse(row[2]);
            tpm.hasLoop = Int32.Parse(row[3]);
            tpm.countLoop = Int32.Parse(row[4]);
            tpm.countnewReqNcCovered = Int32.Parse(row[5]);
            tpm.countReqNcCovered = Int32.Parse(row[6]);
            tpm.nodeCoverage = Double.Parse(row[7]);
            tpm.countnewReqPpcCovered = Int32.Parse(row[8]);
            tpm.countReqPcCovered = Int32.Parse(row[9]);
            tpm.primePathCoverage = Double.Parse(row[10]);

            return tpm;
        }
    }
}
