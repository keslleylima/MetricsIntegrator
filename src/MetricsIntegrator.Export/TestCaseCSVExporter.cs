using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    class TestCaseCSVExporter
    {
        private string outputPath;
        private Dictionary<string, string[]> mapping;
        private Dictionary<string, SourceCodeMetrics> dictSourceCode;
        private Dictionary<string, SourceTestMetrics> dictSourceTest;
        private List<TestCaseMetrics> listTestCase;

        public TestCaseCSVExporter(string outputPath,
                                    Dictionary<string, string[]> mapping,
                                    Dictionary<string, SourceCodeMetrics> dictSourceCode,
                                    Dictionary<string, SourceTestMetrics> dictSourceTest,
                                    List<TestCaseMetrics> listTestCase)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
            this.listTestCase = listTestCase;
        }

        public void Export()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            string delimiter = ";";
            StringBuilder sb = new StringBuilder();

            if (!File.Exists(outputPath))
                sb.Append("ID;countInput;countLineCode;countLineCodeDecl;countLineCodeExe;" +
                    "countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;" +
                    "maxEssentialKnots;maxNesting;minEssentialKnots;ID;countInput;countLineCode;countLineCodeDecl;" +
                    "countLineCodeExe;countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;maxEssentialKnots;" +
                    "maxNesting;minEssentialKnots;Id;AvgPathLength;HasLoop;AvgCountLoop;CountReqEcCovered;EdgeCoverage;CountReqPcCovered;PrimePathCoverage\n");

            foreach (KeyValuePair<string, string[]> kvp in mapping)
            {
                string KeyCode = kvp.Key;
                string[] keysTest = kvp.Value;

                dictSourceCode.TryGetValue(KeyCode, out SourceCodeMetrics metricsSourceCode);

                foreach (string keyTest in keysTest)
                {

                    dictSourceTest.TryGetValue(keyTest, out SourceTestMetrics metricsSourceTest);

                    //if (metricsSourceTest == null)
                    //{
                    //    Console.WriteLine("Warning: metrics source test is null");
                    //    metricsSourceTest = new SourceTestMetrics();
                    //}

                    foreach (TestCaseMetrics tcMetrics in listTestCase)
                    {
                        if (tcMetrics.id == keyTest)
                        {


                            sb.Append(KeyCode + delimiter + metricsSourceCode.countInput + delimiter + metricsSourceCode.countLineCode
                            + delimiter + metricsSourceCode.countLineCodeDecl + delimiter + metricsSourceCode.countLineCodeExe
                            + delimiter + metricsSourceCode.countOutput + delimiter + metricsSourceCode.countPath
                            + delimiter + metricsSourceCode.countPathLog + delimiter + metricsSourceCode.countStmt + delimiter + metricsSourceCode.countStmtDecl
                            + delimiter + metricsSourceCode.countStmtExe + delimiter + metricsSourceCode.cyclomatic
                            + delimiter + metricsSourceCode.cyclomaticModified + delimiter + metricsSourceCode.cyclomaticStrict
                            + delimiter + metricsSourceCode.essential + delimiter + metricsSourceCode.knots
                            + delimiter + metricsSourceCode.maxEssentialKnots + delimiter + metricsSourceCode.maxNesting
                            + delimiter + metricsSourceCode.minEssentialKnots + delimiter);

                            sb.Append(keyTest + delimiter + metricsSourceTest.countInput + delimiter + metricsSourceTest.countLineCode
                            + delimiter + metricsSourceTest.countLineCodeDecl + delimiter + metricsSourceTest.countLineCodeExe
                            + delimiter + metricsSourceTest.countOutput + delimiter + metricsSourceTest.countPath
                            + delimiter + metricsSourceTest.countPathLog + delimiter + metricsSourceTest.countStmt + delimiter + metricsSourceTest.countStmtDecl
                            + delimiter + metricsSourceTest.countStmtExe + delimiter + metricsSourceTest.cyclomatic
                            + delimiter + metricsSourceTest.cyclomaticModified + delimiter + metricsSourceTest.cyclomaticStrict
                            + delimiter + metricsSourceTest.essential + delimiter + metricsSourceTest.knots
                            + delimiter + metricsSourceTest.maxEssentialKnots + delimiter + metricsSourceTest.maxNesting
                            + delimiter + metricsSourceTest.minEssentialKnots + delimiter);

                            sb.Append(tcMetrics.id + delimiter + tcMetrics.avgPathLength + delimiter
                                + tcMetrics.hasLoop + delimiter + tcMetrics.avgCountLoop + delimiter
                                + tcMetrics.countReqEcCovered + delimiter + tcMetrics.edgeCoverage + delimiter
                                + tcMetrics.countReqPcCovered + delimiter + tcMetrics.primePathCoverage + delimiter + "\n");
                        }
                    }
                }
            }
            File.WriteAllText(outputPath, sb.ToString());
        }
    }
}
