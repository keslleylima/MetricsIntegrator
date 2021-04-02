using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Integrator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsIntegrator.Views
{
    public class ExportView : UserControl
    {
        private MainWindow window;
        private MetricsIntegrationManager integrator;
        private StackPanel pnlMetricsSelection;
        private string inDirectoryChoose;

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
            await AskUserForWhereToSaveExportation();
            
            if (inDirectoryChoose.Length == 0)
                return;

            ISet<string> filterMetrics = new HashSet<string>();

            var cbxMetrics = pnlMetricsSelection.Children.GetEnumerator();

            while (cbxMetrics.MoveNext())
            {
                CheckBox cbx = (CheckBox) cbxMetrics.Current;
                bool isChecked = cbx.IsChecked ?? false;

                if (!isChecked)
                    filterMetrics.Add((string) cbx.Content);
            }

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

        private void OnBack(object sender, RoutedEventArgs e)
        {
            window.NavigateToHomeView();
        }
    }
}
