using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MetricsIntegrator.Integrator;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MetricsIntegrator.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExportView : Page
    {
        private MetricsIntegrationManager integrator;

        public ExportView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            integrator = (MetricsIntegrationManager) e.Parameter;
            CreateMetricsSelector();
        }

        private void CreateMetricsSelector()
        {
            var cbxId = new CheckBox();
            cbxId.Name = "cbxId";
            cbxId.Content = "CHX CRIADO!!!";
            cbxId.IsChecked = true;
            cbxId.IsEnabled = false;
            pnlMetricsSelection.Children.Add(cbxId);

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
            foreach (UIElement uie in pnlMetricsSelection.Children)
            {
                ((CheckBox) uie).IsChecked = true;
            }
        }

        private void OnCheckNone(object sender, RoutedEventArgs e)
        {
            foreach (UIElement uie in pnlMetricsSelection.Children)
            {
                if (((CheckBox) uie).Name != "cbxId")
                    ((CheckBox) uie).IsChecked = false;
            }
            }

        private async void OnExport(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("CSV", new List<string>() { ".csv" });
            savePicker.SuggestedFileName = "Metrics";
            StorageFile file = await savePicker.PickSaveFileAsync();
            
            if (file != null)
            {
                ISet<string> filterMetrics = new HashSet<string>();

                foreach (UIElement uie in pnlMetricsSelection.Children)
                {
                    CheckBox cbx = (CheckBox) uie;
                    bool isChecked = cbx.IsChecked ?? false;

                    if (!isChecked)
                        filterMetrics.Add((string) cbx.Content);
                }

                string output = integrator.DoExportation(file.Path, filterMetrics);

                var dialog = new Windows.UI.Popups.MessageDialog("The file has been successfully exported!");
                await dialog.ShowAsync();

                this.Frame.Navigate(typeof(EndView), output);
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
