using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class TestCaseMetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string filepath;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParser(string filepath)
        {
            this.filepath = filepath;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public List<TestCaseMetrics> Parse()
        {
            List<TestCaseMetrics> metrics = new List<TestCaseMetrics>();
            string[] testCaseMetricsFile = File.ReadAllLines(filepath);
            foreach (string line in testCaseMetricsFile.Skip(1).ToArray())
            {
                TestCaseMetrics testCase = new TestCaseMetrics();
                metrics.Add(SetTestCaseMetrics(testCase, line.Split(";")));
            }

            return metrics;
        }

        private TestCaseMetrics SetTestCaseMetrics(TestCaseMetrics tcm, string[] row)
        {
            tcm.id = row[0];
            tcm.avgPathLength = Double.Parse(row[1]);
            tcm.hasLoop = Int32.Parse(row[2]);
            tcm.avgCountLoop = Double.Parse(row[3]);
            tcm.countReqEcCovered = Int32.Parse(row[4]);
            tcm.edgeCoverage = Double.Parse(row[5]);
            tcm.countReqPcCovered = Int32.Parse(row[6]);
            tcm.primePathCoverage = Double.Parse(row[7]);

            return tcm;
        }
    }
}
