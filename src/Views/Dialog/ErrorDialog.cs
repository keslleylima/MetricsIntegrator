using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Platform;
using System;
using System.Reflection;
using MetricsIntegrator.Style.Color;
using System.IO;

namespace MetricsIntegrator.Views.Dialog
{
    public class ErrorDialog : AboutAvaloniaDialog
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly int LEFT_WIDTH = 80;
        private static readonly int WIDTH = 400;
        private static readonly int HEIGHT = 200;
        private readonly string message;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public ErrorDialog(string message)
        {
            Title = "Metrics Integrator - Error";
            this.message = message;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public override void Show()
        {
            BuildWindow();
            BuildContent();
            
            base.Show();
        }

        private void BuildWindow()
        {
            Icon = new WindowIcon(BuildAppIcon());

            Width = WIDTH;
            MinWidth = WIDTH;
            MaxWidth = WIDTH;
            
            Height = HEIGHT;
            MinHeight = HEIGHT;
            MaxHeight = HEIGHT;
            
            CanResize = false;
        }

        private void BuildContent()
        {
            Content = BuildMessageDialog();
        }

        private StackPanel BuildMessageDialog()
        {
            StackPanel dialog = BuildDialog();

            dialog.Children.Add(BuildLeftPanel());
            dialog.Children.Add(BuildRightPanel());

            return dialog;
        }

        private StackPanel BuildDialog()
        {
            StackPanel dialog = new StackPanel();
            
            dialog.Orientation = Avalonia.Layout.Orientation.Horizontal;
            dialog.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            dialog.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            dialog.Width = WIDTH;
            dialog.Height = HEIGHT;
            dialog.Background = ColorBrushFactory.Black();

            return dialog;
        }

        private StackPanel BuildLeftPanel()
        {
            StackPanel pnlLeft = new StackPanel();
            
            pnlLeft.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            pnlLeft.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            pnlLeft.Width = LEFT_WIDTH;
            pnlLeft.Height = HEIGHT;
            pnlLeft.Children.Add(BuildErrorIcon());

            return pnlLeft;
        }

        private Image BuildErrorIcon()
        {
            Image icon = new Image();
            
            icon.Height = 40;
            icon.Margin = new Thickness(0, (HEIGHT / 2) - 20, 0, 0);
            icon.Source = GenerateBitmapOf("/Assets/Images/Signal/error.png");

            return icon;
        }

        private StackPanel BuildRightPanel()
        {
            StackPanel pnlRight = new StackPanel();
            
            pnlRight.Width = WIDTH - LEFT_WIDTH;
            pnlRight.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            pnlRight.Children.Add(BuildMessageArea());
            pnlRight.Children.Add(BuildOkButton()) ;

            return pnlRight;
        }

        private TextBlock BuildMessageArea()
        {
            TextBlock tbMessageArea = new TextBlock();
            
            tbMessageArea.Text = message;
            tbMessageArea.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
            tbMessageArea.Height = HEIGHT - 56;
            tbMessageArea.Margin = new Thickness(10, 10, 10, 10);
            tbMessageArea.Foreground = ColorBrushFactory.White();

            return tbMessageArea;
        }

        private Button BuildOkButton()
        {
            Button btnOk = new Button();
            
            btnOk.Content = "OK";
            btnOk.Width = 100;
            btnOk.Height = 28;
            btnOk.FontSize = 14;
            btnOk.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            btnOk.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            btnOk.Click += (o, e) => this.Close();

            return btnOk;
        }

        private Avalonia.Media.Imaging.Bitmap BuildAppIcon()
        {
            return GenerateBitmapOf("/Assets/Images/icon.png");
        }

        private Avalonia.Media.Imaging.Bitmap GenerateBitmapOf(string path)
        {
            Uri uri = new Uri($"avares://{GetAssemblyName()}{path}");
            
            return new Avalonia.Media.Imaging.Bitmap(OpenAsset(uri));
        }

        private string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }

        private Stream OpenAsset(Uri uri)
        {
            IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            
            return assets.Open(uri);
        }
    }
}
