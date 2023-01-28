using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp2.GameElements;
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int SnakeSquareSize = 20;

        private List<Cell> Cells = new List<Cell>();
        private List<Cell> NewCells = new List<Cell>();

        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        //(x.X + SnakeSquareSize == cell.X &&
        //                x.Y + SnakeSquareSize == cell.Y &&
        //                x.IsLive == true) ||
        //                (x.X - SnakeSquareSize == cell.X &&
        //                x.Y - SnakeSquareSize == cell.Y &&
        //                x.IsLive == true) ||
        //                (x.X + SnakeSquareSize == cell.X &&
        //                x.Y - SnakeSquareSize == cell.Y &&
        //                x.IsLive == true) ||
        //                (x.X - SnakeSquareSize == cell.X &&
        //                x.Y + SnakeSquareSize == cell.Y &&
        //                x.IsLive == true)

        private void timer_Tick(object sender, EventArgs e)
        {
            NewCells = new List<Cell>();

            foreach (Cell cell in Cells)
            {
                List<Cell> NewCellCells = Cells.Where(x =>
                        (cell.X + SnakeSquareSize == x.X &&
                        cell.Y == x.Y &&
                        x.IsLive == true) ||
                        (cell.X == x.X &&
                        cell.Y - SnakeSquareSize == x.Y &&
                        x.IsLive == true) ||
                        (cell.X - SnakeSquareSize == x.X &&
                        cell.Y == x.Y &&
                        x.IsLive == true) ||
                        (cell.X == x.X &&
                        cell.Y + SnakeSquareSize == x.Y &&
                        x.IsLive == true) ||
                        (cell.X + SnakeSquareSize == x.X &&
                        cell.Y + SnakeSquareSize == x.Y &&
                        x.IsLive == true) ||
                        (cell.X - SnakeSquareSize == x.X &&
                        cell.Y - SnakeSquareSize == x.Y &&
                        x.IsLive == true) ||
                        (cell.X + SnakeSquareSize == x.X &&
                        cell.Y - SnakeSquareSize == x.Y &&
                        x.IsLive == true) ||
                        (cell.X - SnakeSquareSize == x.X &&
                        cell.Y + SnakeSquareSize == x.Y &&
                        x.IsLive == true)).ToList();

                if (1 == 3 && cell.IsLive == false)
                {
                    cell.IsLive = true;
                }
                else if (1 < 2 || 1 > 3)
                {
                    cell.IsLive = false;
                }
                else if (cell.IsLive == true && (1 == 2 || 1 == 3))
                {
                    cell.IsLive = true;
                }

                GameArea.Children.Remove(cell.UiElement);

                if (cell.IsLive == true)
                    cell.UiElement = new Rectangle
                    {
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.Black,
                        StrokeThickness = 0.5,
                        Stroke = Brushes.Black
                    };
                else
                    cell.UiElement = new Rectangle
                    {
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.White,
                        StrokeThickness = 0.5,
                        Stroke = Brushes.Black
                    };

                NewCells.Add(cell);
            }

            foreach (Cell item in NewCells)
            {
                AddCellInGameArea(item);
            }
        }

        private void DrawGameArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                Cell essence = new Cell()
                {
                    Y = nextY,
                    X = nextX
                };

                AddCellInGameArea(essence);

                Cells.Add(essence);

                nextIsOdd = !nextIsOdd;
                nextX += SnakeSquareSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SnakeSquareSize;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
            }
        }

        private void DrawEssences(int X, int Y)
        {


            Cell essence = Cells.FirstOrDefault(x => x.X - 20 <= X && x.X >= X &&
                                                x.Y - 20 <= Y && x.Y >= Y);
            if (essence == null) return;

            Cell essenceNew = new Cell()
            {
                X = essence.X - 20,
                Y = essence.Y - 20,
                IsLive = true,
                UiElement = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Black,
                    StrokeThickness = 0.5,
                    Stroke = Brushes.Black
                }
            };

            AddCellInGameArea(essenceNew);

            Cells.Add(essenceNew);
        }

        private void AddCellInGameArea(Cell essence)
        {
            GameArea.Children.Add(essence.UiElement);
            Canvas.SetTop(essence.UiElement, essence.Y);
            Canvas.SetLeft(essence.UiElement, essence.X);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 120);

            if (e.Key == Key.Tab)
            {
                timer.Stop();
            }

            if (e.Key == Key.Space)
            {
                timer.Start();
            }
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawEssences(Convert.ToInt32(e.GetPosition(GameArea).X), Convert.ToInt32(e.GetPosition(GameArea).Y));
        }
    }
}
