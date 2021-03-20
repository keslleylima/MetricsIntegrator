using MetricsIntegrator.Metrics;
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
        private List<MetricsContainer> obtained;
        private List<MetricsContainer> expected;
        private MetricsContainer metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetricsParserTest()
        {
            basePath = GenerateBasePath();
            metrics = new MetricsContainer();
            expected = new List<MetricsContainer>();
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
            metrics = new MetricsContainer();
        }

        private void DoParsing()
        {
            TestCaseMetricsParser parser = new TestCaseMetricsParser(basePath + filename);
            obtained = parser.Parse();
        }

        private void AssertParsingIsCorrect()
        {
            Assert.Equal(expected, obtained);
        }
    }
}
