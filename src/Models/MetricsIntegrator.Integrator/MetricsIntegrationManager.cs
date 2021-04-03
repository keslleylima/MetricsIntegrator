using MetricsIntegrator.Data;
using MetricsIntegrator.Export;
using MetricsIntegrator.IO;
using MetricsIntegrator.Parser;
using System.Collections.Generic;
using System.IO;

namespace MetricsIntegrator.Integrator
{
    /// <summary>
    ///     Responsible for integrating metrics of the following files:
    ///     
    ///     <list type="bullet">
    ///         <item>Source code metrics</item>
    ///         <item>Mapping of tested methods along with the test method that tests it</item>
    ///         <item>Test path metrics</item>
    ///         <item>Test case metrics</item>
    ///     </list>
    /// </summary>
    public class MetricsIntegrationManager
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly static string DELIMITER = ";";
        private readonly string projectName;
        private readonly MetricsParseManager metricsParseManager;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsIntegrationManager(string projectName, 
                                         MetricsFileManager metricsFileManager)
        {
            this.projectName = projectName;
            metricsParseManager = new MetricsParseManager(
                metricsFileManager, 
                DELIMITER
            );
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public MetricsParseManager DoParsing()
        {
            metricsParseManager.Parse();
            
            return metricsParseManager;
        }

        public string DoExportation(string outputDirectory,
                                    FilterMetrics filterMetrics)
        {
            string outputPath = outputDirectory + Path.DirectorySeparatorChar + "results";

            IExporter exportManager = new MetricsExportManager.Builder()
                .OutputDirectory(outputPath)
                .ProjectName(projectName)
                .Mapping(metricsParseManager.Mapping)
                .SourceCodeMetrics(metricsParseManager.SourceCodeMetrics)
                .TestCodeMetrics(metricsParseManager.TestCodeMetrics)
                .TestPathMetrics(metricsParseManager.TestPathMetrics)
                .TestCaseMetrics(metricsParseManager.TestCaseMetrics)
                .FilterMetrics(filterMetrics)
                .Build();

            exportManager.Export();

            return outputPath;
        }
    }
}
