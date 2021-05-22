using MetricsIntegrator.Data;
using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class SourceCodeMetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private readonly Dictionary<string, List<string>> mapping;
        private string filename;
        private string currentMethod;
        private Dictionary<string, Metrics> sourceCodeMetricsObtained;
        private Metrics expectedSourceCodeMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public SourceCodeMetricsParserTest()
        {
            basePath = GenerateBasePath();
            mapping = new Dictionary<string, List<string>>();
            sourceCodeMetricsObtained = new Dictionary<string, Metrics>();
            expectedSourceCodeMetrics = new Metrics("id");
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("sc-test.csv");
            UsingMapping("pkgname1.pkgname2.ClassName1.testedMethod1()", "pkgname3.ClassName2.testMethod1()");

            WithSignature("pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindMetric("id", "pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindMetric("field1", "Method");
            BindMetric("field2", "1");
            BindMetric("field3", "1");

            DoParsing();
            
            AssertSourceCodeMetricsIsCorrect();
        }

        [Fact]
        public void TestConstructorWithNullFilePath()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new SourceCodeMetricsParser(null, mapping);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new SourceCodeMetricsParser("", mapping);
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new SourceCodeMetricsParser("foo/bar.csv", mapping);
            });
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private string GenerateBasePath()
        {
            return  PathManager.GetResourcesPath()
                    + Path.DirectorySeparatorChar
                    + "MetricsIntegrator.Parser"
                    + Path.DirectorySeparatorChar;
        }

        private void UsingFile(string filename)
        {
            this.filename = filename;
        }

        private void UsingMapping(string testedInvoked, params string[] testMethods)
        {
            mapping.Add(testedInvoked, new List<string>(testMethods));
        }

        private void WithSignature(string signature)
        {
            currentMethod = signature;
        }

        private void BindMetric(string metricName, string metricValue)
        {
            expectedSourceCodeMetrics.AddMetric(metricName, metricValue);
        }

        private void DoParsing()
        {
            SourceCodeMetricsParser parser = new SourceCodeMetricsParser(basePath + filename, mapping);
            parser.Parse();

            sourceCodeMetricsObtained = parser.SourceCodeMetrics;
        }

        private void AssertSourceCodeMetricsIsCorrect()
        {
            sourceCodeMetricsObtained.TryGetValue(currentMethod, out Metrics obtainedMetrics);

            Assert.Equal(expectedSourceCodeMetrics, obtainedMetrics);

            expectedSourceCodeMetrics = new Metrics("id");
        }
    }
}
