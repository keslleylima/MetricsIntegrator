using MetricsIntegrator.Integrator;
using MetricsIntegrator.IO;
using System;
using System.IO;

namespace MetricsIntegrator
{
    class Program
    {
        static void Main(string[] args)
        {
            //MetricsIntegrationManager integrator = new MetricsIntegrationManager(
            //    Directory.GetCurrentDirectory(),
            //    AskUserForProjectName(),
            //    CreateMetricsFileManager(args)
            //);

            MetricsIntegrationManager integrator = new MetricsIntegrationManager(
                workingDirectory,
                projectName,
                CreateMetricsFileManager()
            );

            integrator.IntegrateMetrics();
        }

        private static MetricsFileManager CreateMetricsFileManager(string[] args)
        {
            MetricsFileManager metricsFileManager = new MetricsFileManager();

            metricsFileManager.MapPath;
            metricsFileManager.SourceCodePath;
            metricsFileManager.TestCasePath;
            metricsFileManager.TestPathsPath;

            return metricsFileManager;
        }
    }
}
