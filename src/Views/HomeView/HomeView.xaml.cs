using MetricsIntegrator.Integrator;
using MetricsIntegrator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MetricsIntegrator.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
            btnIntegrate.IsEnabled = false;
            inProjectName.TextChanged += (o, e) => { CheckIfIntegrateIsAvailable(); };
        }

        private async void OnChooseMapping(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                inMapping.Text = file.Path;
                CheckIfIntegrateIsAvailable();
            }
            else
            {
                inMapping.Text = "";
            }
        }

        private void CheckIfIntegrateIsAvailable()
        {
            btnIntegrate.IsEnabled = AreAllFilesProvided();
        }

        private bool AreAllFilesProvided()
        {
            return (inProjectName.Text != "")
                && (inMapping.Text != "")
                && (inSourceCode.Text != "")
                && (inTestPath.Text != "")
                && (inTestCase.Text != "");
        }

        private async void OnClearProjectName(object sender, RoutedEventArgs e)
        {
            inProjectName.Text = "";
        }
        
        private async void OnChooseSourceCode(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                inSourceCode.Text = file.Path;
                CheckIfIntegrateIsAvailable();
            }
        }

        private async void OnChooseTestPath(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                inTestPath.Text = file.Path;
                CheckIfIntegrateIsAvailable();
            }
            else
            {
                inTestPath.Text = "";
            }
        }

        private async void OnChooseTestCase(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                inTestCase.Text = file.Path;
                CheckIfIntegrateIsAvailable();
            }
            else
            {
                inTestCase.Text = "";
            }
        }

        private async void OnClear(object sender, RoutedEventArgs e)
        {
            inProjectName.Text = "";
            inMapping.Text = "";
            inSourceCode.Text = "";
            inTestPath.Text = "";
            inTestCase.Text = "";
            btnIntegrate.IsEnabled = false;
        }

        private async void OnIntegrate(object sender, RoutedEventArgs e)
        {
            MetricsIntegrationManager integrator = new MetricsIntegrationManager(
                inProjectName.Text,
                CreateMetricsFileManager()
            );

            this.Frame.Navigate(typeof(ExportView), integrator);
        }

        private MetricsFileManager CreateMetricsFileManager()
        {
            MetricsFileManager metricsFileManager = new MetricsFileManager();

            metricsFileManager.MapPath = inMapping.Text;
            metricsFileManager.SourceCodePath = inSourceCode.Text;
            metricsFileManager.TestCasePath = inTestCase.Text;
            metricsFileManager.TestPathsPath = inTestPath.Text;

            return metricsFileManager;
        }
    }
}