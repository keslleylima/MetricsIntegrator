using MetricsIntegrator.IO;
using MetricsIntegrator.Data;
using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class MetricsParseManagerTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private string testedInvoked;
        private string testMethod;
        private string mapFile;
        private string scFile;
        private string codeCoverageFile;
        private IDictionary<string, Metrics> sourceCodeObtained;
        private IDictionary<string, Metrics> testCodeObtained;
        private IDictionary<string, List<Metrics>> codeCoverageObtained;
        private IDictionary<string, List<string>> mappingObtained;
        private IDictionary<string, List<string>> expectedMapping;
        private List<Metrics> expectedMetrics;
        private Metrics metrics;
       

        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParseManagerTest()
        {
            expectedMapping = new Dictionary<string, List<string>>();
            expectedMetrics = new List<Metrics>();
            metrics = new Metrics();
            basePath = GenerateBasePath();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingMappingFile("map-test.csv");
            UsingSourceCodeMetricsFile("sc-test.csv");
            UsingCodeCoverageFile("tc-test.csv");

            DoParsing();

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod1()");
            WithMetric("Name", "pkgname1.pkgname2.ClassName1.testedMethod1()");
            WithMetric("field1", "Method");
            WithMetric("field2", "1");
            WithMetric("field3", "1");
            BindMetrics();
            AssertSourceCodeMetricsIsCorrect();

            WithTestMethod("pkgname3.ClassName2.testMethod1()");
            WithMetric("Name", "pkgname3.ClassName2.testMethod1()");
            WithMetric("field1", "Method");
            WithMetric("field2", "1");
            WithMetric("field3", "1");
            BindMetrics();
            AssertTestCodeMetricsIsCorrect();

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindTestMethods("pkgname3.ClassName2.testMethod1()", "pkgname3.ClassName2.testMethod2()");
            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod2(SomeClass<T>, SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");
            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod3(SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");
            AssertMappingIsCorrect();

            WithTestMethod("pkgname3.ClassName2.testMethod1()");
            WithMetric("id", "pkgname3.ClassName2.testMethod1()");
            WithMetric("field1", "1");
            WithMetric("field2", "2");
            BindMetrics();
            AssertCodeCoverageIsCorrect();
        }

        [Fact]
        public void TestConstructorWithNullMetricsParseManager()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new MetricsParseManager(null, ";");
            });
        }

        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MetricsParseManager(new MetricsFileManager(), null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MetricsParseManager(new MetricsFileManager(), "");
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

        private void UsingMappingFile(string filepath)
        {
            mapFile = filepath;
        }

        private void UsingSourceCodeMetricsFile(string filepath)
        {
            scFile = filepath;
        }

        private void UsingCodeCoverageFile(string filepath)
        {
            codeCoverageFile = filepath;
        }

        private void DoParsing()
        {
            MetricsParseManager parser = new MetricsParseManager(CreateMetricsFileManager(), ";");

            parser.Parse();

            sourceCodeObtained = parser.SourceCodeMetrics;
            testCodeObtained = parser.TestCodeMetrics;
            codeCoverageObtained = parser.CodeCoverage;
            mappingObtained = parser.Mapping;
        }

        private MetricsFileManager CreateMetricsFileManager()
        {
            return new MetricsFileManager
            {
                MapPath = basePath + mapFile,
                SourceCodePath = basePath + scFile,
                CodeCoveragePath = basePath + codeCoverageFile,
            };
        }

        private void AssertMappingIsCorrect()
        {
            Assert.Equal(expectedMapping, mappingObtained);

            expectedMapping = new Dictionary<string, List<string>>();
        }

        private void AssertCodeCoverageIsCorrect()
        {
            IDictionary<string, List<Metrics>> expected = new Dictionary<string, List<Metrics>>();

            expected.Add(testMethod, expectedMetrics);

            Assert.Equal(expected, codeCoverageObtained);

            expectedMetrics = new List<Metrics>();
        }

        private void WithTestedInvoked(string signature)
        {
            testedInvoked = signature;
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

        private void AssertSourceCodeMetricsIsCorrect()
        {
            Dictionary<string, Metrics> metrics = new Dictionary<string, Metrics>();
            metrics.Add(testedInvoked, expectedMetrics[0]);

            Assert.Equal(metrics, sourceCodeObtained);

            expectedMetrics = new List<Metrics>();
        }

        private void WithTestMethod(string signature)
        {
            testMethod = signature;
        }

        private void AssertTestCodeMetricsIsCorrect()
        {
            Dictionary<string, Metrics> metrics = new Dictionary<string, Metrics>();
            metrics.Add(testMethod, expectedMetrics[0]);

            Assert.Equal(metrics, testCodeObtained);

            expectedMetrics = new List<Metrics>();
        }

        private void BindTestMethods(params string[] testMethods)
        {
            expectedMapping.Add(testedInvoked, new List<string>(testMethods));
        }
    }
}
