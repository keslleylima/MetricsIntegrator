using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Controllers;
using MetricsIntegrator.Style.Color;

namespace MetricsIntegrator.Views
{
    public class HomeView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly HomeController homeController;
        private TextBox inMapping;
        private TextBox inSourceCode;
        private TextBox inCodeCoverage;
        private Button btnIntegrate;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeView()
        {
            homeController = default!;
            inMapping = default!;
            inSourceCode = default!;
            inCodeCoverage = default!;
            btnIntegrate = default!;

            InitializeComponent();
            BuildChooseMappingButton();
            BuildChooseSourceButton();
            BuildChooseCodeCoverageButton();
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

        private void BuildChooseCodeCoverageButton()
        {
            Button btnChooseCodeCoverage = this.FindControl<Button>("btnChooseCodeCoverage");

            btnChooseCodeCoverage.Background = ColorBrushFactory.ThemeAccent();
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
            inCodeCoverage = this.FindControl<TextBox>("inCodeCoverage");
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
            return (inMapping.Text != "")
                && (inMapping.Text != null)
                && (inSourceCode.Text != "")
                && (inSourceCode.Text != null)
                && (inCodeCoverage.Text != "")
                && (inCodeCoverage.Text != null);
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

        private async void OnChooseSourceCode(object sender, RoutedEventArgs e)
        {
            inSourceCode.Text = await homeController.AskUserForMetricsFile();
            CheckIfIntegrateIsAvailable();
        }

        private async void OnChooseCodeCoverage(object sender, RoutedEventArgs e)
        {
            inCodeCoverage.Text = await homeController.AskUserForMetricsFile();
            CheckIfIntegrateIsAvailable();
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            inMapping.Text = "";
            inSourceCode.Text = "";
            inCodeCoverage.Text = "";
            btnIntegrate.IsEnabled = false;
        }

        private void OnIntegrate(object? sender, RoutedEventArgs e)
        {
            homeController.OnIntegrate(
                inMapping.Text, 
                inSourceCode.Text,
                inCodeCoverage.Text
            );
        }
    }
}
