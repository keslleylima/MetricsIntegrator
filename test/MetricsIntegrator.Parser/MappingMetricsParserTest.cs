using MetricsIntegratorTest;
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
        private string filename;
        private string testedInvoked;
        private Dictionary<string, List<string>> obtained;
        private Dictionary<string, List<string>> expected;


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

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod2(SomeClass<T>, SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");

            WithTestedInvoked("pkgname1.pkgname2.ClassName1.testedMethod3(SomeClass2...)");
            BindTestMethods("pkgname3.ClassName2.testMethod1()");

            DoParsing();

            AssertParseIsCorrect();
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
            this.testedInvoked = signature;
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
