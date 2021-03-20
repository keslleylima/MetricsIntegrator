using MetricsIntegrator.Metrics;
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
        private string filename;
        private string testedInvoked;
        private string testMethod;
        private bool isMetricOfTestedInvoked;
        private Dictionary<string, List<string>> mapping;
        private Dictionary<string, MetricsContainer> sourceCodeMetricsObtained;
        private Dictionary<string, MetricsContainer> testCodeMetricsObtained;
        private MetricsContainer expectedSourceCodeMetrics;
        private MetricsContainer expectedTestCodeMetrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public SourceCodeMetricsParserTest()
        {
            basePath = GenerateBasePath();
            isMetricOfTestedInvoked = false;
            mapping = new Dictionary<string, List<string>>();
            sourceCodeMetricsObtained = new Dictionary<string, MetricsContainer>();
            testCodeMetricsObtained = new Dictionary<string, MetricsContainer>();
            expectedSourceCodeMetrics = new MetricsContainer();
            expectedTestCodeMetrics = new MetricsContainer();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("sc-test.csv");
            UsingMapping("pkgname1.pkgname2.ClassName1.testedMethod1()", "pkgname3.ClassName2.testMethod1()");

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindMetric("field1", "Method");
            BindMetric("field2", "1");
            BindMetric("field3", "1");
            
            WithTestMethod("pkgname3.ClassName2.testMethod1()");
            BindMetric("field1", "Method");
            BindMetric("field2", "1");
            BindMetric("field3", "1");

            DoParsing();

            AssertSourceCodeMetricsIsCorrect();
            AssertTestCodeMetricsIsCorrect();
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

        [Fact]
        public void TestConstructorWithNullMapping()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new SourceCodeMetricsParser(basePath + "sc-test.csv", null);
            });
        }
        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new SourceCodeMetricsParser(basePath + "sc-test.csv", mapping, null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new SourceCodeMetricsParser(basePath + "sc-test.csv", mapping, "");
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

        private void WithTestedInvoked(string signature)
        {
            testedInvoked = signature;
            isMetricOfTestedInvoked = true;
        }

        private void BindMetric(string metricName, string metricValue)
        {
            if (isMetricOfTestedInvoked)
            {
                expectedSourceCodeMetrics.AddMetric(metricName, metricValue);
            }
            else
            {
                expectedTestCodeMetrics.AddMetric(metricName, metricValue);
            }
        }

        private void WithTestMethod(string signature)
        {
            testMethod = signature;
            isMetricOfTestedInvoked = false;
        }

        private void DoParsing()
        {
            SourceCodeMetricsParser parser = new SourceCodeMetricsParser(basePath + filename, mapping);
            parser.Parse();

            sourceCodeMetricsObtained = parser.DictSourceCode;
            testCodeMetricsObtained = parser.DictSourceTest;
        }

        private void AssertSourceCodeMetricsIsCorrect()
        {
            Dictionary<string, MetricsContainer> expectedMetrics = new Dictionary<string, MetricsContainer>();
            expectedMetrics.Add(testedInvoked, expectedSourceCodeMetrics);

            Assert.Equal(expectedMetrics, sourceCodeMetricsObtained);
        }

        private void AssertTestCodeMetricsIsCorrect()
        {
            Dictionary<string, MetricsContainer> expectedMetrics = new Dictionary<string, MetricsContainer>();
            expectedMetrics.Add(testMethod, expectedTestCodeMetrics);

            Assert.Equal(expectedMetrics, testCodeMetricsObtained);
        }
    }
}
