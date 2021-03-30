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
            MetricsIntegrationManager integrator = new MetricsIntegrationManager(
                Directory.GetCurrentDirectory(),
                AskUserForProjectName(),
                CreateMetricsFileManager(args)
            );

            integrator.IntegrateMetrics();
        }

        private static string AskUserForProjectName()
        {
            Console.Write("Project name: ");

            return Console.ReadLine();
        }

        private static MetricsFileManager CreateMetricsFileManager(string[] args)
        {
            MetricsFileManager metricsFileManager = new MetricsFileManager();

            if (args.Length > 1)
                metricsFileManager.SetFilesFromCLI(args);
            else
                metricsFileManager.FindAllFromDirectory(Directory.GetCurrentDirectory());

            return metricsFileManager;
        }
    }
}
