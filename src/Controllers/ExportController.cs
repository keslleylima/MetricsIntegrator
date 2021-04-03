using Avalonia.Controls;
using MetricsIntegrator.Data;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Parser;
using MetricsIntegrator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsIntegrator.Controllers
{
    public class ExportController
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MainWindow window;
        private MetricsIntegrationManager integrator;
        private string inDirectoryChoose;


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
        public List<string> TestCaseFieldKeys { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void ParseMetrics()
        {
            MetricsParseManager parser = integrator.DoParsing();
            
            SourceCodeFieldKeys = parser.SourceCodeFieldKeys;
            TestCaseFieldKeys = parser.TestCaseFieldKeys;
        }

        public void OnExport(string outputPath, FilterMetrics filterMetrics)
        {
            if (inDirectoryChoose.Length == 0)
                return;

            string output = integrator.DoExportation(inDirectoryChoose, filterMetrics);

            window.NavigateToEndView(output);
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
