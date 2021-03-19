using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Parser
{
    public class MappingMetricsParserTest
    {
        [Fact]
        public void TestParse()
        {
            string basePath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar 
                + "MetricsIntegrator.Parser" + Path.DirectorySeparatorChar + "map-test.csv";
            
            MappingMetricsParser parser = new MappingMetricsParser(basePath);
            Dictionary<string, List<string>> obtained = parser.Parse();
            Dictionary<string, List<string>> expected = new Dictionary<string, List<string>>()
            {
                { "pkgname1.pkgname2.ClassName1.testedMethod1()", new List<string>() { "pkgname3.ClassName2.testMethod1()", "pkgname3.ClassName2.testMethod2()" } },
                { "pkgname1.pkgname2.ClassName1.testedMethod2(SomeClass<T>, SomeClass2...)", new List<string>() { "pkgname3.ClassName2.testMethod1()" } },
                { "pkgname1.pkgname2.ClassName1.testedMethod3(SomeClass2...)", new List<string>() { "pkgname3.ClassName2.testMethod1()" } }
            };

            Assert.Equal(expected, obtained);
        }
    }
}
