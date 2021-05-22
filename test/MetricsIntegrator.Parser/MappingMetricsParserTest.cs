using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class MappingMetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string basePath;
        private readonly Dictionary<string, List<string>> expected;
        private Dictionary<string, List<string>> obtained;
        private string filename;
        private string testedInvoked;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MappingMetricsParserTest()
        {
            basePath = GenerateBasePath();
            expected = new Dictionary<string, List<string>>();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingFile("map-test.csv");
            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod1()");
            BindTestMethods("pkgname3.ClassName2.testMethod1()", "pkgname3.ClassName2.testMethod2()");

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod2(SomeClass<T>,SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod3(SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");

            DoParsing();

            AssertParseIsCorrect();
        }

        [Fact]
        public void TestConstructorWithNullFilePath()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new MappingMetricsParser(null, ";");
            });
        }

        [Fact]
        public void TestConstructorWithEmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MappingMetricsParser("", ";");
            });
        }

        [Fact]
        public void TestConstructorWithNonExistentFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MappingMetricsParser("foo/bar.txt");
            });
        }

        [Fact]
        public void TestConstructorWithNullDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MappingMetricsParser(basePath + "map-test.csv", null);
            });
        }

        [Fact]
        public void TestConstructorWithEmptyDelimiter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MappingMetricsParser(basePath + "map-test.csv", "");
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

        private void WithTestedInvoked(string signature)
        {
            testedInvoked = signature;
        }

        private void BindTestMethods(params string[] testMethods)
        {
            expected.Add(testedInvoked, new List<string>(testMethods));
        }

        private void DoParsing()
        {
            MappingMetricsParser parser = new MappingMetricsParser(basePath + filename);

            obtained = parser.Parse();
        }

        private void AssertParseIsCorrect()
        {
            Assert.Equal(expected, obtained);
        }
    }
}
