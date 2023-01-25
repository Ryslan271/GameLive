﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp2.GameElements
{
    public class Cell
    {
        public UIElement UiElement { get; set; } = new Rectangle
        {
            Width = 20,
            Height = 20,
            Fill = Brushes.Black,
            StrokeThickness = 0.5,
            Stroke = Brushes.Black
        };

        public bool IsLive { get; set; } = true;

        public int X { get; set; }
        public int Y { get; set; }

        public int NumberNearbyLiving { get; set; }
    }
}
