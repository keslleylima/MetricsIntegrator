using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Export
{
    public class MetricsExportManager : IExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string outputDir;
        private readonly string projectName;
        private readonly MetricsContainer metricsContainer;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsExportManager(string outputDir, string projectName, 
                                    MetricsContainer metricsContainer)
        {
            this.outputDir = outputDir;
            this.projectName = projectName;
            this.metricsContainer = metricsContainer;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Export()
        {
            IExporter exporter = MetricsExporterFactory.CreateTestPathCSVExporter(
                outputDir,
                projectName,
                metricsContainer
            );

            exporter.Export();
        }
    }
}
