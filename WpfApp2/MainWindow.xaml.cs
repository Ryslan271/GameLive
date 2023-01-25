using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.GameElements;

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

            DrawEssences();

        }

        private void GamePlay()
        {
            NewCells = new List<Cell>();

            foreach (Cell cell in Cells)
            {
                int countCell = Cells.Where(x =>
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
                        x.IsLive == true)).Count();

                if (countCell == 3 || countCell == 2)
                {
                    cell.IsLive = true;
                }
                else
                {
                    cell.IsLive = false;
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

        private void DrawEssences()
        {
            int countEssences = 20;

            while (countEssences >= 0)
            {
                Cell essence = new Cell()
                {
                    Y = rnd.Next(1, 20) * 20,
                    X = rnd.Next(1, 20) * 20,
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

                AddCellInGameArea(essence);

                Cells.Add(essence);

                countEssences--;
            }
        }

        private void AddCellInGameArea(Cell essence)
        {
            GameArea.Children.Add(essence.UiElement);
            Canvas.SetTop(essence.UiElement, essence.Y);
            Canvas.SetLeft(essence.UiElement, essence.X);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                GamePlay();

        }
    }
}
