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
        private List<Metrics> obtained;
        private List<Metrics> expected;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParserTest()
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
            UsingFile("tc-test.csv");

            WithMetric("id", "pkg1.pkg2.ClassName1.testedMethod1(int)");
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
            expected.Add(metrics);
            metrics = new Metrics();
        }

        private void DoParsing()
        {
            BaseMetricsParser parser = new BaseMetricsParser(basePath + filename, ";");
            obtained = parser.Parse();
        }

        private void AssertParsingIsCorrect()
        {
            Assert.Equal(expected, obtained);
        }
    }
}
