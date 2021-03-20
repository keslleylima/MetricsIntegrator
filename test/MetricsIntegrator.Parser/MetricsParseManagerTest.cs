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
        private string tpFile;
        private string tcFile;
        private MetricsContainer metricsContainerObtained;
        private Dictionary<string, List<string>> expectedMapping;
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
            UsingTestPathMetricsFile("tp-test.csv");
            UsingTestCaseMetricsFile("tc-test.csv");

            DoParsing();

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod1()");
            WithMetric("field1", "Method");
            WithMetric("field2", "1");
            WithMetric("field3", "1");
            BindMetrics();
            AssertSourceCodeMetricsIsCorrect();

            WithTestMethod("pkgname3.ClassName2.testMethod1()");
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

            WithMetric("id", "pkg1.pkg2.ClassName1.testedMethod1(int)");
            WithMetric("field1", "1");
            WithMetric("field2", "2");
            BindMetrics();
            AssertTestCaseMetricsAreCorrect();

            WithMetric("id", "pkg1.pkg2.ClassName1.testedMethod1(int)");
            WithMetric("field1", "1");
            WithMetric("field2", "2");
            WithMetric("field3", "3");
            BindMetrics();
            AssertTestPathMetricsAreCorrect();
        }

        [Fact]
        public void TestConstructorWithNullMetricsParseManager()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new MetricsParseManager(null);
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

        private void UsingTestPathMetricsFile(string filepath)
        {
            tpFile = filepath;
        }

        private void UsingTestCaseMetricsFile(string filepath)
        {
            tcFile = filepath;
        }

        private void DoParsing()
        {
            MetricsParseManager parser = new MetricsParseManager(CreateMetricsFileManager());

            parser.Parse();

            metricsContainerObtained = parser.MetricsContainer;
        }

        private MetricsFileManager CreateMetricsFileManager()
        {
            return new MetricsFileManager
            {
                MapPath = basePath + mapFile,
                SourceCodePath = basePath + scFile,
                TestCasePath = basePath + tcFile,
                TestPathsPath = basePath + tpFile
            };
        }

        private void AssertMappingIsCorrect()
        {
            Assert.Equal(expectedMapping, metricsContainerObtained.Mapping);

            expectedMapping = new Dictionary<string, List<string>>();
        }

        private void AssertTestCaseMetricsAreCorrect()
        {
            Assert.Equal(expectedMetrics, metricsContainerObtained.TestCaseMetrics);

            expectedMetrics = new List<Metrics>();
        }

        private void AssertTestPathMetricsAreCorrect()
        {
            Assert.Equal(expectedMetrics, metricsContainerObtained.TestPathMetrics);

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

            Assert.Equal(metrics, metricsContainerObtained.SourceCodeMetrics);

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

            Assert.Equal(metrics, metricsContainerObtained.TestCodeMetrics);

            expectedMetrics = new List<Metrics>();
        }

        private void BindTestMethods(params string[] testMethods)
        {
            expectedMapping.Add(testedInvoked, new List<string>(testMethods));
        }
    }
}
