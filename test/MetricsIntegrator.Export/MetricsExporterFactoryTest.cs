using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Export
{
    public class MetricsExporterFactoryTest
    {
        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestExport()
        {
            MetricsExporterFactory factory = new MetricsExporterFactory.Builder()
                .ProjectName()
                .OutputDirectory()
                .BaseMetrics()
                .Mapping()
                .SourceCodeMetrics()
                .TestCodeMetrics()
                .UsingDelimiter()
                .Build();

            IExporter tcExporter = factory.CreateTestCaseCSVExporter(metrics);
            tcExporter.Export();
            
            IExporter tpExporter = factory.CreateTestPathCSVExporter(metrics);
            tpExporter.Export();
        }
    }
}
