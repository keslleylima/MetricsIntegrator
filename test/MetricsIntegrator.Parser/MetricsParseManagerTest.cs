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
        private string coveredMethod;
        private string testMethod;
        private string mapFile;
        private string scFile;
        private string codeCoverageFile;
        private IDictionary<string, Metrics> sourceCodeObtained;
        private IDictionary<string, Metrics> codeCoverageObtained;
        private IDictionary<string, List<string>> mappingObtained;
        private IDictionary<string, List<string>> expectedMapping;
        private Metrics expectedMetrics;
        private Metrics metrics;
       

        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParseManagerTest()
        {
            expectedMapping = new Dictionary<string, List<string>>();
            expectedMetrics = default!;
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

            WithTestAndCoveredMethod(
               "pkg.ClassName2.testMethod1()",
               "pkg.ClassName2.testedMethod1()"
           );
            WithCoveredMethod("pkgname1.pkgname2.ClassName1.testedMethod1()");
            WithMetric("id", "pkgname1.pkgname2.ClassName1.testedMethod1()");
            WithMetric("field1", "Method");
            WithMetric("field2", "1");
            WithMetric("field3", "1");
            BindMetrics();
            AssertSourceCodeMetricsIsCorrect();

            WithCoveredMethod("pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindTestMethods("pkgname3.ClassName2.testMethod1()", "pkgname3.ClassName2.testMethod2()");
            WithCoveredMethod("pkgname1.pkgname2.ClassName1.testedMethod2(SomeClass<T>,SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");
            WithCoveredMethod("pkgname1.pkgname2.ClassName1.testedMethod3(SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");
            AssertMappingIsCorrect();

            WithTestAndCoveredMethod(
               "pkg.ClassName2.testMethod1()",
               "pkg.ClassName2.testedMethod1()"
           );
            WithMetric("TestMethod", "pkg.ClassName2.testMethod1()");
            WithMetric("TestedMethod", "pkg.ClassName2.testedMethod1()");
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

        private void WithTestAndCoveredMethod(string testMethod, string coveredMethod)
        {
            metrics = new Metrics(testMethod + ";" + coveredMethod);
        }

        private void AssertCodeCoverageIsCorrect()
        {
            codeCoverageObtained.TryGetValue(
                expectedMetrics.GetID(), 
                out Metrics obtainedMetrics
            );

            Assert.Equal(expectedMetrics, obtainedMetrics);

            expectedMetrics = default!;
        }

        private void WithCoveredMethod(string signature)
        {
            coveredMethod = signature;
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

        private void AssertSourceCodeMetricsIsCorrect()
        {
            sourceCodeObtained.TryGetValue(
                coveredMethod, 
                out Metrics obtained
            );

            Assert.Equal(expectedMetrics, obtained);

            expectedMetrics = default!;
        }

        private void WithTestMethod(string signature)
        {
            testMethod = signature;
        }

        private void BindTestMethods(params string[] testMethods)
        {
            expectedMapping.Add(coveredMethod, new List<string>(testMethods));
        }
    }
}
