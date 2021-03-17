using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    class TestPathCSVExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string outputPath;
        private Dictionary<string, string[]> mapping;
        private Dictionary<string, SourceCodeMetrics> dictSourceCode;
        private Dictionary<string, SourceTestMetrics> dictSourceTest;
        private List<TestPathMetrics> listTestPath;


        //---------------------------------------------------------------------
        //		Constructor
        //----------------------------------------------------------------------
        public TestPathCSVExporter(string outputPath,
                                    Dictionary<string, string[]> mapping,
                                    Dictionary<string, SourceCodeMetrics> dictSourceCode,
                                    Dictionary<string, SourceTestMetrics> dictSourceTest,
                                    List<TestPathMetrics> listTestPath)
        {
            this.outputPath = outputPath;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
            this.listTestPath = listTestPath;
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
                sb.Append(
                    // METRICAS METODO TESTADO
                    "ID;countInput;countLineCode;countLineCodeDecl;countLineCodeExe;" +
                    "countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;" +
                    "maxEssentialKnots;maxNesting;minEssentialKnots;" +

                    // METRICAS METODO QUE O TESTA
                    "ID;countInput;countLineCode;countLineCodeDecl;" +
                    "countLineCodeExe;countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;maxEssentialKnots;" +
                    "maxNesting;minEssentialKnots;" +

                    // METRICAS TEST CASE
                    "id;testPath;pathLength;hasLoop;countLoop;countnewReqEcCovered;" +
                    "countReqEcCovered;EdgeCoverage;countnewReqPpcCovered;countReqPcCovered;primePathCoverage" + "\n");
            foreach (KeyValuePair<string, string[]> kvp in mapping)
            {
                string testedMethod = kvp.Key;
                string[] testMethods = kvp.Value;

                dictSourceCode.TryGetValue(testedMethod, out SourceCodeMetrics metricsSourceCode);

                foreach (string testMethod in testMethods)
                {

                    dictSourceTest.TryGetValue(testMethod, out SourceTestMetrics metricsSourceTest);

                    foreach (TestPathMetrics tpMetrics in listTestPath)
                    {
                        if (tpMetrics.id == testMethod)
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

                            sb.Append(testMethod + delimiter + metricsSourceTest.countInput + delimiter + metricsSourceTest.countLineCode
                            + delimiter + metricsSourceTest.countLineCodeDecl + delimiter + metricsSourceTest.countLineCodeExe
                            + delimiter + metricsSourceTest.countOutput + delimiter + metricsSourceTest.countPath
                            + delimiter + metricsSourceTest.countPathLog + delimiter + metricsSourceTest.countStmt + delimiter + metricsSourceTest.countStmtDecl
                            + delimiter + metricsSourceTest.countStmtExe + delimiter + metricsSourceTest.cyclomatic
                            + delimiter + metricsSourceTest.cyclomaticModified + delimiter + metricsSourceTest.cyclomaticStrict
                            + delimiter + metricsSourceTest.essential + delimiter + metricsSourceTest.knots
                            + delimiter + metricsSourceTest.maxEssentialKnots + delimiter + metricsSourceTest.maxNesting
                            + delimiter + metricsSourceTest.minEssentialKnots + delimiter);

                            sb.Append(tpMetrics.id + delimiter + tpMetrics.testPath + delimiter
                                + tpMetrics.pathLength + delimiter + tpMetrics.hasLoop + delimiter
                                + tpMetrics.countLoop + delimiter + tpMetrics.countnewReqNcCovered + delimiter
                                + tpMetrics.countReqNcCovered + delimiter + tpMetrics.nodeCoverage + delimiter
                                + tpMetrics.countnewReqPpcCovered + delimiter + tpMetrics.countReqPcCovered + delimiter
                                + tpMetrics.primePathCoverage + delimiter + "\n");

                        }
                    }
                }
            }
            File.WriteAllText(outputPath, sb.ToString());
        }
    }
}
