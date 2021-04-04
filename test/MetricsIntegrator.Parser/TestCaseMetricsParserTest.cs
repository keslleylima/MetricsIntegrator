using MetricsIntegrator.Data;
using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class TestCaseMetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private string filename;
        private IDictionary<string, List<Metrics>> obtained;
        private List<Metrics> expectedMetrics;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParserTest()
        {
            basePath = GenerateBasePath();
            metrics = new Metrics();
            expectedMetrics = new List<Metrics>();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("tc-test.csv");

            WithMetric("id", "pkgname3.ClassName2.testMethod1()");
            WithMetric("field1", "1");
            WithMetric("field2", "2");
            BindMetrics();

            DoParsing();

            AssertParsingIsCorrect();
        }

        [Fact]
        public void TestConstructorWithNullFilePath()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new BaseMetricsParser(null, ";");
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new BaseMetricsParser("", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new BaseMetricsParser("foo/bar.csv", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new BaseMetricsParser(basePath + "tc-test.csv", null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new BaseMetricsParser(basePath + "tc-test.csv", "");
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

        private void WithMetric(string metricName, string metricValue)
        {
            metrics.AddMetric(metricName, metricValue);
        }

        private void BindMetrics()
        {
            expectedMetrics.Add(metrics);
            metrics = new Metrics();
        }

        private void DoParsing()
        {
            BaseMetricsParser parser = new BaseMetricsParser(basePath + filename, ";");
            obtained = parser.Parse();
        }

        private void AssertParsingIsCorrect()
        {
            Dictionary<string, List<Metrics>> expected = new Dictionary<string, List<Metrics>>();
            expected.Add(expectedMetrics[0].GetID(), expectedMetrics);

            Assert.Equal(expected, obtained);
        }
    }
}
