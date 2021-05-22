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
        private Metrics expectedMetrics;
        private IDictionary<string, Metrics> obtained;
        private string filename;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public CodeCoverageParserTest()
        {
            basePath = GenerateBasePath();
            metrics = default!;
            expectedMetrics = default!;
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("tc-test.csv");

            WithTestAndCoveredMethod(
                "pkg.ClassName2.testMethod1()", 
                "pkg.ClassName2.testedMethod1()"
            );
            WithMetric("TestMethod", "pkg.ClassName2.testMethod1()");
            WithMetric("TestedMethod", "pkg.ClassName2.testedMethod1()");
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
                new CodeCoverageMetricsParser(null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser("");
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CodeCoverageMetricsParser("foo/bar.csv");
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

        private void WithTestAndCoveredMethod(string testMethod, string coveredMethod)
        {
            metrics = new Metrics(testMethod + ";" + coveredMethod);
        }

        private void WithMetric(string metricName, string metricValue)
        {
            metrics.AddMetric(metricName, metricValue);
        }

        private void BindMetrics()
        {
            expectedMetrics = metrics;
            metrics = default!;
        }

        private void DoParsing()
        {
            CodeCoverageMetricsParser parser = new CodeCoverageMetricsParser(basePath + filename);
            obtained = parser.Parse();
        }

        private void AssertParsingIsCorrect()
        {
            obtained.TryGetValue(expectedMetrics.GetID(), out Metrics obtainedMetrics);

            Assert.Equal(expectedMetrics, obtainedMetrics);
        }
    }
}
