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

            Dictionary<string, string[]> mapping = new Dictionary<string, string[]>();
            string[] metricsMappingFile = FileUtils.ReadAllLines(mapPath);

            foreach (string line in metricsMappingFile)
            {
                string[] column;
                column = line.Split(";");
                mapping.Add(column[0], column[1..column.Length]);
            }

            List<TestPathMetrics> listTestPath = new List<TestPathMetrics>();
            string[] testPathMetricsFile = FileUtils.ReadAllLines(testPathsPath);
            foreach (string line in testPathMetricsFile.Skip(1).ToArray())
            {
                TestPathMetrics testPath = new TestPathMetrics();
                string[] column;
                column = line.Split(";");
                listTestPath.Add(SetTestPathMetrics(testPath, column));
            }

            List<TestCaseMetrics> listTestCase = new List<TestCaseMetrics>();
            string[] testCaseMetricsFile = FileUtils.ReadAllLines(testCasePath);
            foreach (string line in testCaseMetricsFile.Skip(1).ToArray())
            {
                TestCaseMetrics testCase = new TestCaseMetrics();
                string[] column;
                column = line.Split(";");
                listTestCase.Add(SetTestCaseMetrics(testCase, column));
            }

            Dictionary<string, SourceCodeMetrics> dictSourceCode = new Dictionary<string, SourceCodeMetrics>();
            Dictionary<string, SourceTestMetrics> dictSourceTest = new Dictionary<string, SourceTestMetrics>();
            string[] sourceMetricsFile = FileUtils.ReadAllLines(smPath);

            foreach (string line in sourceMetricsFile.Skip(1).ToArray())
            {
                string[] column;
                column = line.Split(";");
                if (mapping.ContainsKey(column[1]))
                {
                    SourceCodeMetrics metricsSourceCode = new SourceCodeMetrics();
                    SetSourceCodeMetrics(metricsSourceCode, column);
                    dictSourceCode.Add(column[1], metricsSourceCode);
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
                                dictSourceTest.Add(column[1], metricsSourceTest);

                            }
                        }

                    }

                }
            }
            // To comment the lines 177 end 180 if you don't want to generate the dataset at test path level.
            string TestPathFilePath = basePath + @"\TP_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            CsvIntegratorTestPath(TestPathFilePath, mapping, dictSourceCode, dictSourceTest, listTestPath);

            string TestCaseFilePath = basePath + @"\TC_dataset_resulting_" + projectPath.Substring(projectPath.LastIndexOf(@"\") + 1) + ".csv";
            CsvIntegratorTestCase(TestCaseFilePath, mapping, dictSourceCode, dictSourceTest, listTestCase);
        }

        public static void SetSourceCodeMetrics(SourceCodeMetrics msc, string[] row)
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
        public static void SetSourceTestMetrics(SourceTestMetrics mst, string[] row)
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

        public static TestPathMetrics SetTestPathMetrics(TestPathMetrics tpm, string[] row)
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
        public static TestCaseMetrics SetTestCaseMetrics(TestCaseMetrics tcm, string[] row)
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



















        public static void CsvIntegratorTestPath(string filePath, Dictionary<string, string[]> mapping, Dictionary<string, SourceCodeMetrics> dictSourceCode,
            Dictionary<string, SourceTestMetrics> dictSourceTest, List<TestPathMetrics> listTestPath)
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