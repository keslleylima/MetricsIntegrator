using MetricsIntegrator.Parser;
using MetricsIntegrator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetricsIntegrator
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = @"C:\Users\william\Documents\workspace\vsstudio\MetricsIntegrator";
            string projectPath = basePath + @"\jopt"; //For example, @"D:\GitHub\TCmetricsGenerator\Projects\Cli"
            
            string[] csvFilesPath = FileUtils.GetAllFilesFromDirectoryEndingWith(projectPath, "csv");
            string smPath = string.Empty;
            string mapPath = string.Empty;
            string testPathsPath = string.Empty;
            string testCasePath = string.Empty;
            foreach (string csvPath in csvFilesPath)
            {
                if (csvPath.Contains("SCM_"))
                    smPath = csvPath;

                if (csvPath.Contains("MAP_"))
                    mapPath = csvPath;

                if (csvPath.Contains("TestPath_"))
                    testPathsPath = csvPath;

                if (csvPath.Contains("TestCase_"))
                    testCasePath = csvPath;
            }

            MappingMetricsParser mapParser = new MappingMetricsParser(mapPath);
            Dictionary<string, string[]> mapping = mapParser.Parse();

            TestPathMetricsParser tpParser = new TestPathMetricsParser(testPathsPath);
            List<TestPathMetrics> listTestPath = tpParser.Parse();

            TestCaseMetricsParser tcParser = new TestCaseMetricsParser(testCasePath);
            List<TestCaseMetrics> listTestCase = tcParser.Parse();

            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(smPath);
            scmParser.Parse();

            string TestPathFilePath = basePath + @"\TP_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            CsvIntegratorTestPath(TestPathFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestPath);

            string TestCaseFilePath = basePath + @"\TC_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            CsvIntegratorTestCase(TestCaseFilePath, mapping, scmParser.DictSourceCode, scmParser.DictSourceTest, listTestCase);
        }

        
        



















        public static void CsvIntegratorTestPath(string filePath, 
                                                 Dictionary<string, string[]> mapping, 
                                                 Dictionary<string, SourceCodeMetrics> dictSourceCode,
                                                 Dictionary<string, SourceTestMetrics> dictSourceTest, 
                                                 List<TestPathMetrics> listTestPath)
        {

            if (File.Exists(filePath))
                File.Delete(filePath);

            string delimiter = ";";
            StringBuilder sb = new StringBuilder();

            if (!File.Exists(filePath))
                sb.Append("ID;countInput;countLineCode;countLineCodeDecl;countLineCodeExe;" +
                    "countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;" +
                    "maxEssentialKnots;maxNesting;minEssentialKnots;ID;countInput;countLineCode;countLineCodeDecl;" +
                    "countLineCodeExe;countOutput;countPath;countPathLog;countStmt;countStmtDec;" +
                    "countStmtExe;cyclomatic;cyclomaticModified;cyclomaticStrict;essential;knots;maxEssentialKnots;" +
                    "maxNesting;minEssentialKnots;id;testPath;pathLength;hasLoop;countLoop;countnewReqEcCovered;" +
                    "countReqEcCovered;EdgeCoverage;countnewReqPpcCovered;countReqPcCovered;primePathCoverage" + "\n");
            foreach (KeyValuePair<string, string[]> kvp in mapping)
            {
                string KeyCode = kvp.Key;
                string[] keysTest = kvp.Value;

                dictSourceCode.TryGetValue(KeyCode, out SourceCodeMetrics metricsSourceCode);

                foreach (string keyTest in keysTest)
                {

                    dictSourceTest.TryGetValue(keyTest, out SourceTestMetrics metricsSourceTest);

                    foreach (TestPathMetrics tpMetrics in listTestPath)
                    {
                        if (tpMetrics.id == keyTest)
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
            File.WriteAllText(filePath, sb.ToString());
        }

        public static void CsvIntegratorTestCase(string filePath, Dictionary<string, string[]> mapping, Dictionary<string, SourceCodeMetrics> dictSourceCode,
            Dictionary<string, SourceTestMetrics> dictSourceTest, List<TestCaseMetrics> listTestCase)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            string delimiter = ";";
            StringBuilder sb = new StringBuilder();

            if (!File.Exists(filePath))
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
            File.WriteAllText(filePath, sb.ToString());

        }
    }
}