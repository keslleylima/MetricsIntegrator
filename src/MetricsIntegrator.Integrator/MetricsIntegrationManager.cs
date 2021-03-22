using MetricsIntegrator.Export;
using MetricsIntegrator.IO;
using MetricsIntegrator.Parser;
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
            DoParsing();
            DoExportation();
        }

        private void DoParsing()
        {
            metricsParseManager.Parse();
        }

        private void DoExportation()
        {
            IExporter exportManager = new MetricsExportManager.Builder()
                .OutputDirectory(workingDirectory + Path.DirectorySeparatorChar + "results")
                .ProjectName(projectName)
                .Mapping(metricsParseManager.Mapping)
                .SourceCodeMetrics(metricsParseManager.SourceCodeMetrics)
                .TestCodeMetrics(metricsParseManager.TestCodeMetrics)
                .TestPathMetrics(metricsParseManager.TestPathMetrics)
                .TestCaseMetrics(metricsParseManager.TestCaseMetrics)
                .Build();

            exportManager.Export();
        }
    }
}
