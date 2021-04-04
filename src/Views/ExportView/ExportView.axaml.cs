using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Controllers;
using MetricsIntegrator.Data;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.Parser;
using MetricsIntegrator.Views.Dialog;
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
        private StackPanel pnlTestPathMetrics;
        private string inDirectoryChoose;
        private ExportController exportController;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public ExportView()
        {
            InitializeComponent();
            pnlSourceCodeMetrics = this.FindControl<StackPanel>("pnlSrcCodeMetrics");
            pnlTestPathMetrics = this.FindControl<StackPanel>("pnlTestPathMetrics");
            pnlTestCaseMetrics = this.FindControl<StackPanel>("pnlTestCaseMetrics");
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

                BuildSourceCodeMetricsSelector(exportController.SourceCodeFieldKeys);
                BuildTestPathMetricsSelector(exportController.TestPathFieldKeys);
                BuildTestCaseMetricsSelector(exportController.TestCaseFieldKeys);
            }
            catch (Exception e)
            {
                ErrorDialog dialog = new ErrorDialog(e.Message);
                dialog.Show();
            }
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

        private void BuildTestPathMetricsSelector(List<string> fieldKeys)
        {
            BuildCheckBoxColumn(pnlTestPathMetrics, fieldKeys);
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
            CheckAllTestPathMetrics();
            CheckAllTestCaseMetrics();
        }

        private void CheckAllSourceCodeMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void CheckAllTestPathMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlTestPathMetrics.Children.GetEnumerator());
        }

        private void CheckAllTestCaseMetrics()
        {
            CheckAllCheckBoxFromEnumerator(pnlTestCaseMetrics.Children.GetEnumerator());
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
            UncheckAllTestPathMetrics();
            UncheckAllTestCaseMetrics();
        }

        private void UncheckAllSourceCodeMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlSourceCodeMetrics.Children.GetEnumerator());
        }

        private void UncheckAllTestPathMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlTestPathMetrics.Children.GetEnumerator());
        }

        private void UncheckAllTestCaseMetrics()
        {
            UncheckAllCheckBoxFromEnumerator(pnlTestCaseMetrics.Children.GetEnumerator());
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
            
            //try
            //{
                exportController.OnExport(outputPath, filter);
            //}
            //catch (Exception e2)
            //{
            //    ErrorDialog dialog = new ErrorDialog(e2.ToString());
            //    dialog.Show();
            //}
        }

        private FilterMetrics ParseUnselectedMetrics()
        {
            FilterMetrics filterMetrics = new FilterMetrics();

            ParseUnselectedSourceCodeMetrics(filterMetrics);
            ParseUnselectedTestPathMetrics(filterMetrics);
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

        private void ParseUnselectedTestPathMetrics(FilterMetrics filterMetrics)
        {
            var cbxMetrics = pnlTestPathMetrics.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                CheckBox cbx = (CheckBox)cbxMetrics.Current;
                bool isChecked = cbx.IsChecked ?? false;

                if (!isChecked)
                    filterMetrics.AddTestPathFilter((string) cbx.Content);
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
            exportController.OnBack();
        }
    }
}
