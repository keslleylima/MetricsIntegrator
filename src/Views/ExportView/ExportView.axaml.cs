using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Controllers;
using MetricsIntegrator.Data;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Views.Dialog;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Views
{
    public class ExportView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly StackPanel pnlSourceCodeMetrics;
        private readonly StackPanel pnlCodeCoverage;
        private readonly ExportController exportController;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public ExportView()
        {
            InitializeComponent();
            pnlSourceCodeMetrics = this.FindControl<StackPanel>("pnlSrcCodeMetrics");
            pnlCodeCoverage = this.FindControl<StackPanel>("pnlCodeCoverage");
        }

        public ExportView(MainWindow window, MetricsIntegrationManager integrator) 
            : this()
        {
            exportController = new ExportController(window, integrator);
 
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
            try
            {
                exportController.ParseMetrics();

                BuildSourceCodeMetricsSelector(
                    exportController.SourceCodeFieldKeys, 
                    exportController.SourceCodeIdentifierKey
                );
                
                BuildCodeCoverageSelector(
                    exportController.CodeCoverageFieldKeys, 
                    exportController.CodeCoverageIdentifierKey
                );
            }
            catch (Exception e)
            {
                ErrorDialog dialog = new ErrorDialog(e.Message);
                dialog.Show();
            }
        }

        private void BuildSourceCodeMetricsSelector(List<string> fieldKeys, string id)
        {
            BuildCheckBoxColumn(pnlSourceCodeMetrics, fieldKeys, id);
        }

        private void BuildCheckBoxColumn(StackPanel panel, List<string> labels, string id)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                if (labels[i] == id)
                    panel.Children.Add(CreateCheckBoxForIdField(labels[i]));
                else
                    panel.Children.Add(CreateCheckBoxForField(labels[i]));
            }
        }

        private void BuildCodeCoverageSelector(List<string> fieldKeys, string id)
        {
            BuildCheckBoxColumn(pnlCodeCoverage, fieldKeys, id);
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
            CheckAllCodeCoverage();
        }

        private void CheckAllSourceCodeMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void CheckAllCodeCoverage()
        {
            CheckAllCheckBoxFromEnumerator(pnlCodeCoverage.Children.GetEnumerator());
        }

        private void CheckAllCheckBoxFromEnumerator(AvaloniaList<IControl>.Enumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                CheckBox cbx = (CheckBox) enumerator.Current;

                if (!cbx.IsEnabled)
                    continue;
                
                cbx.IsChecked = true;
            }
        }

        private void OnCheckNone(object sender, RoutedEventArgs e)
        {
            UncheckAllSourceCodeMetrics();
            UncheckAllCodeCoverage();
        }

        private void UncheckAllSourceCodeMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void UncheckAllCodeCoverage()
        {
            UncheckAllCheckBoxFromEnumerator(pnlCodeCoverage.Children.GetEnumerator());
        }

        private void UncheckAllCheckBoxFromEnumerator(AvaloniaList<IControl>.Enumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                CheckBox cbx = (CheckBox) enumerator.Current;

                if (!cbx.IsEnabled)
                    continue;

                cbx.IsChecked = false;
            }
        }

        private async void OnExport(object sender, RoutedEventArgs e)
        {
            string outputPath = await exportController.AskUserForWhereToSaveExportation();
            FilterMetrics filter = ParseUnselectedMetrics();

            try
            {
                exportController.OnExport(outputPath, filter);
            }
            catch (ArgumentException)
            {
                ErrorDialog dialog = new ErrorDialog(
                    "No metrics after parsing.\n\n" +
                    "Please check for missing data. If there is no missing data, " +
                    "then it is not possible to generate metrics with the data " +
                    "provided ;(. Try to provide more data in the metrics files."
                );

                dialog.Show();
            }
            catch (Exception e2)
            {
                ErrorDialog dialog = new ErrorDialog(e2.ToString());
                dialog.Show();
            }
        }

        private FilterMetrics ParseUnselectedMetrics()
        {
            FilterMetrics filterMetrics = new FilterMetrics();

            ParseUnselectedSourceCodeMetrics(filterMetrics);
            ParseUnselectedCodeCoverage(filterMetrics);

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

        private void ParseUnselectedCodeCoverage(FilterMetrics filterMetrics)
        {
            var cbxMetrics = pnlCodeCoverage.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                CheckBox cbx = (CheckBox)cbxMetrics.Current;
                bool isChecked = cbx.IsChecked ?? false;

                if (!isChecked)
                    filterMetrics.AddCodeCoverageFilter((string) cbx.Content);
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            exportController.OnBack();
        }
    }
}
