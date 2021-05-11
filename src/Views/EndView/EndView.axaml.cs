using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace MetricsIntegrator.Views
{
    public class EndView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly MainWindow window;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public EndView()
        {
            window = default!;

            InitializeComponent();
        }

        public EndView(MainWindow window, string output) : this()
        {
            this.window = window;

            TextBlock lblExportPath = this.FindControl<TextBlock>("lblExportPath");
            
            lblExportPath.Text = output;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnGoBackToHome(object sender, RoutedEventArgs e)
        {
            window.NavigateToHomeView();
        }

        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
