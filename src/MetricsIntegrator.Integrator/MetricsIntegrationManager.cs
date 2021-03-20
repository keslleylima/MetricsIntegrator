using MetricsIntegrator.Export;
using MetricsIntegrator.IO;
using MetricsIntegrator.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private readonly string workingDirectory;
        private readonly string projectName;
        private readonly MetricsParseManager metricsParseManager;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsIntegrationManager(string workingDirectory, string projectName, 
                                         MetricsFileManager metricsFileManager)
        {
            this.workingDirectory = workingDirectory;
            this.projectName = projectName;
            metricsParseManager = new MetricsParseManager(metricsFileManager);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void IntegrateMetrics()
        {
            metricsParseManager.Parse();

            string outputDir = workingDirectory + Path.DirectorySeparatorChar + "results";
            string delimiter = ";";
            
            MetricsExportManager exportManager = new MetricsExportManager(
                outputDir, projectName, delimiter, metricsParseManager.Mapping, 
                metricsParseManager.DictSourceCode, metricsParseManager.DictSourceTest
            );
            exportManager.ExportTestCaseMetrics(metricsParseManager.ListTestCase);
            exportManager.ExportTestPathMetrics(metricsParseManager.ListTestPath);
        }
    }
}
