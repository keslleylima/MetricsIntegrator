using MetricsIntegrator.Metric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    class TestCaseCSVExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string outputPath;
        private Dictionary<string, string[]> mapping;
        private Dictionary<string, SourceCodeMetrics> dictSourceCode;
        private Dictionary<string, Metrics> dictSourceTest;
        private List<TestCaseMetrics> listTestCase;
        private string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseCSVExporter(string outputPath,
                                    Dictionary<string, string[]> mapping,
                                    Dictionary<string, SourceCodeMetrics> dictSourceCode,
                                    Dictionary<string, Metrics> dictSourceTest,
                                    List<TestCaseMetrics> listTestCase,
                                    string delimiter)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
            this.listTestCase = listTestCase;
            this.delimiter = delimiter;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            string delimiter = ";";
            StringBuilder sb = new StringBuilder();

            if (!File.Exists(outputPath))
                WriteHeader(sb);

            foreach (KeyValuePair<string, string[]> kvp in mapping)
            {
                string testedMethod = kvp.Key;
                string[] testMethods = kvp.Value;

                dictSourceCode.TryGetValue(testedMethod, out SourceCodeMetrics metricsSourceCode);

                foreach (string testMethod in testMethods)
                {
                    dictSourceTest.TryGetValue(testMethod, out Metrics metricsSourceTest);

                    // ???
                    //if (metricsSourceTest == null)
                    //{
                    //    Console.WriteLine("Warning: metrics source test is null");
                    //    metricsSourceTest = new SourceTestMetrics();
                    //}

                    foreach (TestCaseMetrics tcMetrics in listTestCase)
                    {
                        if (tcMetrics.id == testMethod)
                        {
                            WriteMetricsOfTestedMethod(sb, testedMethod, metricsSourceCode);
                            WriteMetricsOfTestMethod(sb, testMethod, metricsSourceTest);
                            WriteMetricsOfTestCase(sb, tcMetrics);
                        }
                    }
                }
            }
            File.WriteAllText(outputPath, sb.ToString());
        }

        private void WriteHeader(StringBuilder sb)
        {
            WriteTestedMethodMetrics(sb);
            WriteTestMethodMetrics(sb);
            WriteTestCaseMetrics(sb);
        }

        private void WriteTestedMethodMetrics(StringBuilder sb)
        {
            foreach (string metric in GetTestedMethodMetrics())
            {
                sb.Append(metric);
                sb.Append(delimiter);
            }
        }

        private string[] GetTestedMethodMetrics()
        {
            return GetFirstMetricFrom(dictSourceTest).GetMetrics();
        }

        private Metrics GetFirstMetricFrom(Dictionary<string, Metrics> dictionary)
        {
            var dictEnum = dictionary.GetEnumerator();
            dictEnum.MoveNext();
            
            return dictEnum.Current.Value;
        }

        private void WriteTestMethodMetrics(StringBuilder sb)
        {
            foreach (string metric in GetTestMethodMetrics())
            {
                sb.Append(metric);
                sb.Append(delimiter);
            }
        }

        private string[] GetTestMethodMetrics()
        {
            return GetFirstMetricFrom(dictSourceTest).GetMetrics();
        }

        private void WriteTestCaseMetrics(StringBuilder sb)
        {
            foreach (string metric in GetTestCaseMetrics())
            {
                sb.Append(metric);
                sb.Append(delimiter);
            }
        }

        private string[] GetTestCaseMetrics()
        {
            return listTestCase[0].GetMetrics();
        }

        private void WriteMetricsOfTestedMethod(StringBuilder sb, string testedMethod, SourceCodeMetrics metricsSourceCode)
        {
            sb.Append(testedMethod + delimiter + metricsSourceCode.countInput + delimiter + metricsSourceCode.countLineCode
            + delimiter + metricsSourceCode.countLineCodeDecl + delimiter + metricsSourceCode.countLineCodeExe
            + delimiter + metricsSourceCode.countOutput + delimiter + metricsSourceCode.countPath
            + delimiter + metricsSourceCode.countPathLog + delimiter + metricsSourceCode.countStmt + delimiter + metricsSourceCode.countStmtDecl
            + delimiter + metricsSourceCode.countStmtExe + delimiter + metricsSourceCode.cyclomatic
            + delimiter + metricsSourceCode.cyclomaticModified + delimiter + metricsSourceCode.cyclomaticStrict
            + delimiter + metricsSourceCode.essential + delimiter + metricsSourceCode.knots
            + delimiter + metricsSourceCode.maxEssentialKnots + delimiter + metricsSourceCode.maxNesting
            + delimiter + metricsSourceCode.minEssentialKnots + delimiter);
        }

        private void WriteMetricsOfTestMethod(StringBuilder sb, string testMethod, Metrics metricsSourceTest)
        {
            foreach (string metricValue in metricsSourceTest.GetAllMetricValues())
            {
                sb.Append(metricValue);
                sb.Append(delimiter);
            }
        }

        private void WriteMetricsOfTestCase(StringBuilder sb, TestCaseMetrics tcMetrics)
        {
            sb.Append(tcMetrics.id + delimiter + tcMetrics.avgPathLength + delimiter
                        + tcMetrics.hasLoop + delimiter + tcMetrics.avgCountLoop + delimiter
                        + tcMetrics.countReqEcCovered + delimiter + tcMetrics.edgeCoverage + delimiter
                        + tcMetrics.countReqPcCovered + delimiter + tcMetrics.primePathCoverage + delimiter + "\n");
        }
    }
}
