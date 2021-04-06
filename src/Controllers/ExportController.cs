using Avalonia.Controls;
using MetricsIntegrator.Data;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Parser;
using MetricsIntegrator.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsIntegrator.Controllers
{
    /// <summary>
    ///     Responsible for controlling the behavior of 'ExportView'.
    /// </summary>
    public class ExportController
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MainWindow window;
        private MetricsIntegrationManager integrator;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public ExportController(MainWindow window, MetricsIntegrationManager integrator)
        {
            this.window = window;
            this.integrator = integrator;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<string> SourceCodeFieldKeys { get; private set; }
        public List<string> CodeCoverageFieldKeys { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void ParseMetrics()
        {
            MetricsParseManager parser = integrator.DoParsing();
            
            SourceCodeFieldKeys = parser.SourceCodeFieldKeys;
            CodeCoverageFieldKeys = parser.CodeCoverageFieldKeys;
        }

        public void OnExport(string outputDirectory, FilterMetrics filterMetrics)
        {
            if (outputDirectory.Length == 0)
                return;

            string outputPath = integrator.DoExportation(outputDirectory, filterMetrics);

            window.NavigateToEndView(outputPath);
        }

        public async Task<string> AskUserForWhereToSaveExportation()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            string result = await dialog.ShowAsync(window);

            return ((result != null) && (result.Length > 0))
                    ? result
                    : "";
        }

        public void OnBack()
        {
            window.NavigateToHomeView();
        }
    }
}
