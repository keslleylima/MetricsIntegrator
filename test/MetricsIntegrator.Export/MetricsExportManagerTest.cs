using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Export
{
    public class MetricsExportManagerTest
    {
        [Fact]
        public void TestExport()
        {
            MetricsExportManager manager = new MetricsExportManager.Builder()
                .ProjectName()
                .OutputDirectory()
                .BaseMetrics()
                .Mapping()
                .SourceCodeMetrics()
                .TestCodeMetrics()
                .UsingDelimiter()
                .Build();

            manager.Export();
        }
    }
}
