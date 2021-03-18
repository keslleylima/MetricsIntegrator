using MetricsIntegrator.Integrator;
using MetricsIntegrator.IO;
using System;
using System.IO;

namespace MetricsIntegrator
{
    class Program
    {
        /// <summary>
        ///     If no parameter is passed, a search will be made for the files in 
        ///     the current directory, looking for files with the prefixes 'SCM_',
        ///     'TP_', 'TC_' and 'MAP_'. 
        /// </summary>
        /// <param name="args">-scm path -map path -tc path -tp path</param>
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
