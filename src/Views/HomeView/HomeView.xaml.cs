using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.IO;
using MetricsIntegrator.Style.Color;
using MetricsIntegrator.Views.Dialog;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using Image = Avalonia.Controls.Image;

namespace MetricsIntegrator.Views
{
    public class HomeView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private TextBox inProjectName;
        private TextBox inMapping;
        private TextBox inSourceCode;
        private TextBox inTestPath;
        private TextBox inTestCase;
        private Button btnIntegrate;
        private string inFileChoose;
        private MainWindow window;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeView()
        {
            InitializeComponent();
            
            Button btnClearProjectName = this.FindControl<Button>("btnClearProjectName");
            btnClearProjectName.Background = ColorBrushFactory.ThemeAccent();

            btnIntegrate = this.FindControl<Button>("btnIntegrate");
            btnIntegrate.Click += OnIntegrate;
            btnIntegrate.Background = ColorBrushFactory.ThemeAccent();

            Button btnChooseMapping = this.FindControl<Button>("btnChooseMapping");
            btnChooseMapping.Background = ColorBrushFactory.ThemeAccent();

            Button btnChooseSource = this.FindControl<Button>("btnChooseSource");
            btnChooseSource.Background = ColorBrushFactory.ThemeAccent();

            Button btnChooseTestPath = this.FindControl<Button>("btnChooseTestPath");
            btnChooseTestPath.Background = ColorBrushFactory.ThemeAccent();

            Button btnChooseTestCase = this.FindControl<Button>("btnChooseTestCase");
            btnChooseTestCase.Background = ColorBrushFactory.ThemeAccent();


            inMapping = this.FindControl<TextBox>("inMapping");
            inSourceCode = this.FindControl<TextBox>("inSourceCode");
            inTestPath = this.FindControl<TextBox>("inTestPath");
            inTestCase = this.FindControl<TextBox>("inTestCase");

            UserControl pnlHome = this.FindControl<UserControl>("pnlHome");
            pnlHome.Background = ColorBrushFactory.Black();

            inProjectName = this.FindControl<TextBox>("inProjectName");
            inProjectName.KeyUp += (o, e) => { CheckIfIntegrateIsAvailable(); };
        }

        public HomeView(MainWindow window) : this()
        {
            this.window = window;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void CheckIfIntegrateIsAvailable()
        {
            btnIntegrate.IsEnabled = AreAllFilesProvided();
        }

        private bool AreAllFilesProvided()
        {
            return (inProjectName.Text != "") 
                && (inProjectName.Text != null)
                && (inMapping.Text != "")
                && (inMapping.Text != null)
                && (inSourceCode.Text != "")
                && (inSourceCode.Text != null)
                && (inTestPath.Text != "")
                && (inTestPath.Text != null)
                && (inTestCase.Text != "")
                && (inTestCase.Text != null);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnChooseMapping(object sender, RoutedEventArgs e)
        {
            await AskUserForCSVFileOfType("Mapping file");
            inMapping.Text = inFileChoose;
            CheckIfIntegrateIsAvailable();
        }

        private async Task AskUserForCSVFileOfType(string type)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = type, Extensions = { "csv" } });
            dialog.AllowMultiple = false;

            string[] result = await dialog.ShowAsync(window);

            inFileChoose = ((result != null) && (result.Length > 0)) 
                    ? result[0] 
                    : "";
        }

        private void OnClearProjectName(object sender, RoutedEventArgs e)
        {
            inProjectName.Text = "";
        }

        private async void OnChooseSourceCode(object sender, RoutedEventArgs e)
        {
            await AskUserForCSVFileOfType("Metrics file");
            inSourceCode.Text = inFileChoose;
            CheckIfIntegrateIsAvailable();
        }

        private async void OnChooseTestPath(object sender, RoutedEventArgs e)
        {
            await AskUserForCSVFileOfType("Metrics file");
            inTestPath.Text = inFileChoose;
            CheckIfIntegrateIsAvailable();
        }

        private async void OnChooseTestCase(object sender, RoutedEventArgs e)
        {
            await AskUserForCSVFileOfType("Metrics file");
            inTestCase.Text = inFileChoose;
            CheckIfIntegrateIsAvailable();
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            inProjectName.Text = "";
            inMapping.Text = "";
            inSourceCode.Text = "";
            inTestPath.Text = "";
            inTestCase.Text = "";
            btnIntegrate.IsEnabled = false;
        }

        private void OnIntegrate(object sender, RoutedEventArgs e)
        {
            MetricsIntegrationManager integrator = new MetricsIntegrationManager(
                inProjectName.Text,
                CreateMetricsFileManager()
            );

            window.NavigateToExportView(integrator);
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
