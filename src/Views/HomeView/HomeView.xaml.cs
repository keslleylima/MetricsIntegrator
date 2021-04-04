using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using MetricsIntegrator.Controllers;
using MetricsIntegrator.Integrator;
using MetricsIntegrator.IO;
using MetricsIntegrator.Style.Color;
using MetricsIntegrator.Views.Dialog;


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
        private HomeController homeController;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeView()
        {
            InitializeComponent();
            BuildProjectNameInput();
            BuildCleanProjectNameButton();
            BuildChooseMappingButton();
            BuildChooseSourceButton();
            BuildChooseTestPathButton();
            BuildChooseTestCaseButton();
            BuildIntegrateButton();
            FetchInputFields();
            SetBackground();
        }

        public HomeView(MainWindow window) : this()
        {
            homeController = new HomeController(window);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void BuildProjectNameInput()
        {
            inProjectName = this.FindControl<TextBox>("inProjectName");
            inProjectName.KeyUp += (o, e) => { CheckIfIntegrateIsAvailable(); };
        }

        private void BuildCleanProjectNameButton()
        {
            Button btnClearProjectName = this.FindControl<Button>("btnClearProjectName");

            btnClearProjectName.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildChooseMappingButton()
        {
            Button btnChooseMapping = this.FindControl<Button>("btnChooseMapping");

            btnChooseMapping.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildChooseSourceButton()
        {
            Button btnChooseSource = this.FindControl<Button>("btnChooseSource");

            btnChooseSource.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildChooseTestPathButton()
        {
            Button btnChooseTestPath = this.FindControl<Button>("btnChooseTestPath");

            btnChooseTestPath.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildChooseTestCaseButton()
        {
            Button btnChooseTestCase = this.FindControl<Button>("btnChooseTestCase");

            btnChooseTestCase.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildIntegrateButton()
        {
            btnIntegrate = this.FindControl<Button>("btnIntegrate");

            btnIntegrate.Click += OnIntegrate;
            btnIntegrate.Background = ColorBrushFactory.ThemeAccent();
        }

        private void FetchInputFields()
        {
            inMapping = this.FindControl<TextBox>("inMapping");
            inSourceCode = this.FindControl<TextBox>("inSourceCode");
            inTestPath = this.FindControl<TextBox>("inTestPath");
            inTestCase = this.FindControl<TextBox>("inTestCase");
        }

        private void SetBackground()
        {
            UserControl pnlHome = this.FindControl<UserControl>("pnlHome");

            pnlHome.Background = ColorBrushFactory.Black();
        }

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
            inMapping.Text = await homeController.AskUserForMappingFile();
            CheckIfIntegrateIsAvailable();
        }

        private void OnClearProjectName(object sender, RoutedEventArgs e)
        {
            inProjectName.Text = "";
        }

        private async void OnChooseSourceCode(object sender, RoutedEventArgs e)
        {
            inSourceCode.Text = await homeController.AskUserForMetricsFile();
            CheckIfIntegrateIsAvailable();
        }

        private async void OnChooseTestPath(object sender, RoutedEventArgs e)
        {
            inTestPath.Text = await homeController.AskUserForMetricsFile();
            CheckIfIntegrateIsAvailable();
        }

        private async void OnChooseTestCase(object sender, RoutedEventArgs e)
        {
            inTestCase.Text = await homeController.AskUserForMetricsFile();
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
            homeController.OnIntegrate(
                inProjectName.Text, 
                inMapping.Text, 
                inSourceCode.Text, 
                inTestPath.Text,
                inTestCase.Text
            );
        }
    }
}
