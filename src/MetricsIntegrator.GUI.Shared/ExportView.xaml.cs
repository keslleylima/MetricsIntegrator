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
        }

        private async void OnCheckAll(object sender, RoutedEventArgs e)
        {
        }

        private void OnCheckNone(object sender, RoutedEventArgs e)
        {

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
                var dialog = new Windows.UI.Popups.MessageDialog("The file has been successfully exported!");
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(EndView));
            }
            else
            {
                //inTestCase.Text = "";
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
