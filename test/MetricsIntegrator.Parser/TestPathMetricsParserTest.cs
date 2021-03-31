using MetricsIntegrator.Data;
using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class TestPathMetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private string filename;
        private List<Metrics> obtained;
        private List<Metrics> expected;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestPathMetricsParserTest()
        {
            basePath = GenerateBasePath();
            metrics = new Metrics();
            expected = new List<Metrics>();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("tp-test.csv");

            WithMetric("id", "pkg1.pkg2.ClassName1.testedMethod1(int)");
            WithMetric("field1", "1");
            WithMetric("field2", "2");
            WithMetric("field3", "3");
            BindMetrics();

            DoParsing();

            AssertParsingIsCorrect();
        }

        [Fact]
        public void TestConstructorWithNullFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestPathMetricsParser(null, ";");
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestPathMetricsParser("", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestPathMetricsParser("foo/bar.csv", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestPathMetricsParser(basePath + "tc-test.csv", null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestPathMetricsParser(basePath + "tc-test.csv", "");
            });
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private string GenerateBasePath()
        {
            return PathManager.GetResourcesPath()
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
            expected.Add(metrics);
            metrics = new Metrics();
        }

        private void DoParsing()
        {
            BaseMetricsParser parser = new TestPathMetricsParser(basePath + filename);
            obtained = parser.Parse();
        }

        private void AssertParsingIsCorrect()
        {
            Assert.Equal(expected, obtained);
        }
    }
}
