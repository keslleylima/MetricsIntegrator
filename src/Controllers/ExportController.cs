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
        private readonly MainWindow window;
        private readonly MetricsIntegrationManager integrator;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public ExportController(MainWindow window, MetricsIntegrationManager integrator)
        {
            this.window = window;
            this.integrator = integrator;

            SourceCodeFieldKeys = new List<string>();
            CodeCoverageFieldKeys = new List<string>();
            SourceCodeIdentifierKey = default!;
            CodeCoverageIdentifierKey = default!;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<string> SourceCodeFieldKeys { get; private set; }
        public List<string> CodeCoverageFieldKeys { get; private set; }
        public string SourceCodeIdentifierKey { get; private set; }
        public string CodeCoverageIdentifierKey { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void ParseMetrics()
        {
            MetricsParseManager parser = integrator.DoParsing();
            
            SourceCodeFieldKeys = parser.SourceCodeFieldKeys;
            CodeCoverageFieldKeys = parser.CodeCoverageFieldKeys;
            SourceCodeIdentifierKey = parser.SourceCodeIdentifierKey;
            CodeCoverageIdentifierKey = parser.CodeCoverageIdentifierKey;
        }

        public void OnExport(string outputPath, FilterMetrics filterMetrics)
        {
            if (outputPath.Length == 0)
                return;

            integrator.DoExportation(outputPath, filterMetrics);

            window.NavigateToEndView(outputPath);
        }

        public async Task<string> AskUserForSavePath()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.DefaultExtension = "csv";
            dialog.InitialFileName = "Dataset";
            dialog.Title = "Save dataset file";
            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "Dataset file",
                Extensions = { "csv" }
            });

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
