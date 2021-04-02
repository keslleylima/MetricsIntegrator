using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MetricsIntegrator.Integrator;

namespace MetricsIntegrator.Views
{
    public class MainWindow : Window
    {
        private ContentControl contentControl;


        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            contentControl = this.FindControl<ContentControl>("contentControl");
            contentControl.Content = new HomeView(this);
        }

        public void NavigateToHomeView()
        {
            contentControl.Content = new HomeView(this);
        }

        public void NavigateToExportView(MetricsIntegrationManager integrator)
        {
            contentControl.Content = new ExportView(this, integrator);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
