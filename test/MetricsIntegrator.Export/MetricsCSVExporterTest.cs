using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Export
{
    public class MetricsCSVExporterTest
    {
        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestExport()
        {
            IExporter exporter = new MetricsCSVExporter.Builder()
                .OutputPath()
                .BaseMetrics()
                .Mapping()
                .SourceCodeMetrics()
                .TestCodeMetrics()
                .UsingDelimiter()
                .Build();

            exporter.Export();
        }
    }
}
