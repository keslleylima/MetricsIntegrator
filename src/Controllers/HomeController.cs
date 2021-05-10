using Avalonia.Controls;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Views;
using System.Threading.Tasks;
using MetricsIntegrator.IO;

namespace MetricsIntegrator.Controllers
{
    /// <summary>
    ///     Responsible for controlling the behavior of 'HomeView'.
    /// </summary>
    public class HomeController
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly MainWindow window;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeController(MainWindow window)
        {
            this.window = window;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public Task<string> AskUserForMappingFile()
        {
            return AskUserForCSVFileOfType("Mapping file");
        }

        private async Task<string> AskUserForCSVFileOfType(string type)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = type, Extensions = { "csv" } });
            dialog.AllowMultiple = false;

            string[] result = await dialog.ShowAsync(window);

            return ((result != null) && (result.Length > 0))
                    ? result[0]
                    : "";
        }

        public Task<string> AskUserForMetricsFile()
        {
            return AskUserForCSVFileOfType("Metrics file");
        }

        public void OnIntegrate(string mapPath, string sourceCodePath, 
                                string codeCoveragePath)
        {
            MetricsIntegrationManager integrator = new MetricsIntegrationManager(
                CreateMetricsFileManager(mapPath, sourceCodePath, codeCoveragePath)
            );

            window.NavigateToExportView(integrator);
        }

        private MetricsFileManager CreateMetricsFileManager(string mapPath,
                                                            string sourceCodePath,
                                                            string codeCoveragePath)
        {
            MetricsFileManager metricsFileManager = new MetricsFileManager();

            metricsFileManager.MapPath = mapPath;
            metricsFileManager.SourceCodePath = sourceCodePath;
            metricsFileManager.CodeCoveragePath = codeCoveragePath;

            return metricsFileManager;
        }
    }
}
