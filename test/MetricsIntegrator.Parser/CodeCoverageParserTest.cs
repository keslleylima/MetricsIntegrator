using MetricsIntegrator.Data;
using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class CodeCoverageParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private readonly List<Metrics> expectedMetrics;
        private IDictionary<string, List<Metrics>> obtained;
        private string filename;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public CodeCoverageParserTest()
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
                new CodeCoverageMetricsParser(null, ";");
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser("", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser("foo/bar.csv", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser(basePath + "tc-test.csv", null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser(basePath + "tc-test.csv", "");
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
            CodeCoverageMetricsParser parser = new CodeCoverageMetricsParser(basePath + filename, ";");
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
