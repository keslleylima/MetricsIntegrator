using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Data;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Parser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsIntegrator.Views
{
    public class ExportView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MainWindow window;
        private MetricsIntegrationManager integrator;
        private StackPanel pnlSourceCodeMetrics;
        private StackPanel pnlTestCaseMetrics;
        private string inDirectoryChoose;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public ExportView()
        {
            InitializeComponent();
            pnlSourceCodeMetrics = this.FindControl<StackPanel>("pnlSrcCodeMetrics");
            pnlTestCaseMetrics = this.FindControl<StackPanel>("pnlTestCaseMetrics");
        }

        public ExportView(MainWindow window, MetricsIntegrationManager integrator) 
            : this()
        {
            this.window = window;
            this.integrator = integrator;

            BuildMetricsSelector();
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BuildMetricsSelector()
        {
            MetricsParseManager parser = integrator.DoParsing();

            BuildSourceCodeMetricsSelector(parser.SourceCodeFieldKeys);
            BuildTestCaseMetricsSelector(parser.TestCaseFieldKeys);
        }

        private void BuildSourceCodeMetricsSelector(List<string> fieldKeys)
        {
            BuildCheckBoxColumn(pnlSourceCodeMetrics, fieldKeys);
        }

        private void BuildCheckBoxColumn(StackPanel panel, List<string> labels)
        {
            panel.Children.Add(CreateCheckBoxForIdField(labels[0]));

            for (int i = 1; i < labels.Count; i++)
            {
                panel.Children.Add(CreateCheckBoxForField(labels[i]));
            }
        }

        private void BuildTestCaseMetricsSelector(List<string> fieldKeys)
        {
            BuildCheckBoxColumn(pnlTestCaseMetrics, fieldKeys);
        }

        private CheckBox CreateCheckBoxForIdField(string field)
        {
            CheckBox cbx = new CheckBox();

            cbx.Name = "cbxId";
            cbx.Content = field;
            cbx.IsChecked = true;
            cbx.IsEnabled = false;

            return cbx;
        }

        private CheckBox CreateCheckBoxForField(string field)
        {
            CheckBox cbx = new CheckBox();

            cbx.Name = "cbx" + field;
            cbx.Content = field;
            cbx.IsChecked = true;
            cbx.IsEnabled = true;

            return cbx;
        }

        private void OnCheckAll(object sender, RoutedEventArgs e)
        {
            CheckAllSourceCodeMetrics();
            CheckAllTestCaseMetrics();
        }

        private void CheckAllSourceCodeMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void CheckAllTestCaseMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlTestCaseMetrics.Children.GetEnumerator());
        }

        private void CheckAllCheckBoxFromEnumerator(AvaloniaList<IControl>.Enumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                ((CheckBox) enumerator.Current).IsChecked = true;
            }
        }

        private void OnCheckNone(object sender, RoutedEventArgs e)
        {
            UncheckAllSourceCodeMetrics();
            UncheckAllTestCaseMetrics();
        }

        private void UncheckAllSourceCodeMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void UncheckAllTestCaseMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlTestCaseMetrics.Children.GetEnumerator());
        }

        private void UncheckAllCheckBoxFromEnumerator(AvaloniaList<IControl>.Enumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                ((CheckBox) enumerator.Current).IsChecked = false;
            }
        }

        private async void OnExport(object sender, RoutedEventArgs e)
        {
            await AskUserForWhereToSaveExportation();
            
            if (inDirectoryChoose.Length == 0)
                return;

            FilterMetrics filterMetrics = ParseUnselectedMetrics();
            string output = integrator.DoExportation(inDirectoryChoose, filterMetrics);

            window.NavigateToEndView(output);
        }

        private async Task AskUserForWhereToSaveExportation()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            string result = await dialog.ShowAsync(window);

            inDirectoryChoose = ((result != null) && (result.Length > 0))
                    ? result
                    : "";
        }

        private FilterMetrics ParseUnselectedMetrics()
        {
            FilterMetrics filterMetrics = new FilterMetrics();

            ParseUnselectedSourceCodeMetrics(filterMetrics);
            ParseUnselectedTestCaseMetrics(filterMetrics);

            return filterMetrics;
        }

        private void ParseUnselectedSourceCodeMetrics(FilterMetrics filterMetrics)
        {
            var cbxMetrics = pnlSourceCodeMetrics.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                CheckBox cbx = (CheckBox) cbxMetrics.Current;
                bool isChecked = cbx.IsChecked ?? false;

                if (!isChecked)
                    filterMetrics.AddSourceCodeFilter((string) cbx.Content);
            }
        }

        private void ParseUnselectedTestCaseMetrics(FilterMetrics filterMetrics)
        {
            var cbxMetrics = pnlTestCaseMetrics.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                CheckBox cbx = (CheckBox) cbxMetrics.Current;
                bool isChecked = cbx.IsChecked ?? false;

                if (!isChecked)
                    filterMetrics.AddTestCaseFilter((string) cbx.Content);
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            window.NavigateToHomeView();
        }
    }
}
