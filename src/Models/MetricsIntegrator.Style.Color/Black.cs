using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsIntegrator.Style.Color
{
    public class Black
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly string hex = "#242424";
        private static readonly IBrush brush = Avalonia.Media.Brush.Parse(hex);


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private Black()
        {
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public static string Hex { get => hex; }
        public static IBrush Brush { get => brush; }
    }
}
