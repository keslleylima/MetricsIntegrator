using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Integrator;
using System.Collections.Generic;

namespace MetricsIntegrator.Views
{
    public class ExportView : UserControl
    {
        private MainWindow window;
        private MetricsIntegrationManager integrator;
        private StackPanel pnlMetricsSelection;

        public ExportView()
        {
            InitializeComponent();
            pnlMetricsSelection = this.FindControl<StackPanel>("pnlMetricsSelection");
        }

        public ExportView(MainWindow window, MetricsIntegrationManager integrator) : this()
        {
            this.window = window;
            this.integrator = integrator;

            CreateMetricsSelector();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CreateMetricsSelector()
        {
            List<string> fieldKeys = integrator.DoParsing();

            pnlMetricsSelection.Children.Add(CreateCheckBoxForIdField(fieldKeys[0]));

            for (int i = 1; i < fieldKeys.Count; i++)
            {
                pnlMetricsSelection.Children.Add(CreateCheckBoxForField(fieldKeys[i]));
            }
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
            var cbxMetrics = pnlMetricsSelection.Children.GetEnumerator();
            
            while (cbxMetrics.MoveNext())
            {
                ((CheckBox) cbxMetrics.Current).IsChecked = true;
            }
        }

        private void OnCheckNone(object sender, RoutedEventArgs e)
        {
            var cbxMetrics = pnlMetricsSelection.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                if (((CheckBox) cbxMetrics.Current).Name == "cbxId")
                        continue;

                ((CheckBox) cbxMetrics.Current).IsChecked = false;
            }
        }

        private async void OnExport(object sender, RoutedEventArgs e)
        {
            //FileSavePicker savePicker = new FileSavePicker();
            //savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            //savePicker.FileTypeChoices.Add("CSV", new List<string>() { ".csv" });
            //savePicker.SuggestedFileName = "Metrics";
            //StorageFile file = await savePicker.PickSaveFileAsync();

            //if (file != null)
            //{
            //    ISet<string> filterMetrics = new HashSet<string>();

            //    foreach (UIElement uie in pnlMetricsSelection.Children)
            //    {
            //        CheckBox cbx = (CheckBox)uie;
            //        bool isChecked = cbx.IsChecked ?? false;

            //        if (!isChecked)
            //            filterMetrics.Add((string)cbx.Content);
            //    }

            //    string output = integrator.DoExportation(file.Path, filterMetrics);

            //    var dialog = new Windows.UI.Popups.MessageDialog("The file has been successfully exported!");
            //    await dialog.ShowAsync();

            //    this.Frame.Navigate(typeof(EndView), output);
            //}
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            window.NavigateToHomeView();
        }
    }
}
