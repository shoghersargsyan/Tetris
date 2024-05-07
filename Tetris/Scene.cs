using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    internal class Scene
    {
        public Rectangle[,] GameGrid { get; set; }
        public bool[,] IsFilled { get; set; }
        public SolidBrush[,] GridBrushes { get; set; }
        public IFigure CurrentFigure { get; set; }
        public int NumRows { get; set; }
        public int NumCols { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Size { get; set; }
        public int Score { get; set; }
        public bool IsPaused { get; set; }

        Random random;
        readonly BasicFigure[] figures;
        readonly SolidBrush[] figureBrushes = new SolidBrush[] {
                                                        new SolidBrush(Color.Red),
                                                        new SolidBrush(Color.Blue),
                                                        new SolidBrush(Color.Green),
                                                        new SolidBrush(Color.Azure),
                                                        new SolidBrush(Color.Pink),
                                                        new SolidBrush(Color.Brown),
                                                        new SolidBrush(Color.Aqua),
                                                        new SolidBrush(Color.Coral),
                                                        new SolidBrush(Color.Magenta),
                                                        new SolidBrush(Color.Lime),
                                                        new SolidBrush(Color.RoyalBlue),
                                                        new SolidBrush(Color.LawnGreen),
                                                        new SolidBrush(Color.PaleVioletRed),
                                                        new SolidBrush(Color.Salmon),
                                                        new SolidBrush(Color.Chartreuse),
                                                        new SolidBrush(Color.DeepSkyBlue),
                                                        new SolidBrush(Color.Indigo)
                                                        };

        public Scene(int height, int width, int size)
        {
            Height = height;
            Width = width;
            Size = size;
            IsPaused = true;

            NumRows = height / size;
            NumCols = width / size;

            GameGrid = new Rectangle[NumRows, NumCols];
            IsFilled = new bool[NumRows, NumCols];
            GridBrushes = new SolidBrush[NumRows, NumCols];

            figures = new BasicFigure[] {
                                                new SquareFigure(Size, NumCols / 2 * Size),
                                                new LineFigure(Size, NumCols / 2* Size),
                                                new L1Figure(Size, NumCols / 2 * Size),
                                                new L2Figure(Size, NumCols / 2 * Size),
                                                new TFigure(Size, NumCols / 2 * Size),
                                                new S1Figure(Size, NumCols / 2 * Size),
                                                new S2Figure(Size, NumCols / 2* Size),
                                                };

            StartGame();
        }

        public void StartGame()
        {
            Score = 0;

            // Initialize the grid
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumCols; j++)
                {
                    IsFilled[i, j] = false;
                    GameGrid[i, j] = new Rectangle(j * Size, i * Size, Size, Size);
                }
            }

            // choose a random figure with random color
            random = new Random();
            CurrentFigure = figures[random.Next(figures.Length)];
            CurrentFigure.Brush = figureBrushes[random.Next(figureBrushes.Length)];
        }

        bool CheckBottomHit()
        {
            for (int i = 0; i < CurrentFigure.Items.Length; i++)
            {
                // check whether the current figure hits the bottom border of the game.
                if (CurrentFigure.Items[i].Y + Size >= Height)
                {
                    return true;
                }

                int row_index = CurrentFigure.Items[i].Y / Size;
                int col_index = CurrentFigure.Items[i].X / Size;

                // check whether the current figure is on top of a block
                if (IsFilled[row_index + 1, col_index] == true)
                {
                    return true;
                }

            }

            return false;
        }

        public void Step()
        {
            if (CheckBottomHit() == true)
            {
                PutOnGrid();
                CurrentFigure = figures[random.Next(figures.Length)];
                CurrentFigure.Brush = figureBrushes[random.Next(figureBrushes.Length)];
                CurrentFigure.Reset();
                CheckGrid();

                if (CheckOverflow() == true)
                {
                    IsPaused = true;
                    MessageBox.Show("You loose.");
                    IsPaused = false;
                    StartGame();
                }
            }
            else
            {
                CurrentFigure.MoveDown();
            }
        }

        private bool CheckOverflow()
        {
            bool state = false;

            for (int j = 0; j < NumCols; j++)
            {
                if (IsFilled[0, j] == true)
                {
                    state = true;
                    break;
                }
            }

            return state;
        }

        void CheckGrid()
        {
            List<int> selectedRows = new List<int>();

            for (int i = NumRows - 1; i >= 0; i--)
            {
                bool flag = true;

                for (int j = 0; j < NumCols; j++)
                {
                    if (IsFilled[i, j] == false)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag == true)
                {
                    selectedRows.Add(i);
                }
            }

            EraseFullRows(selectedRows);            
        }

        private void EraseFullRows(List<int> rows)
        {

            if (rows.Count == 0)
            {
                return;
            }

            bool[,] isFilledCopy = new bool[NumRows, NumCols];
            SolidBrush[,] gridBrushesCopy = new SolidBrush[NumRows, NumCols];

            int newIndex = NumRows - 1;

            for (int i = NumRows - 1; i >= 0; i--)
            {
                if (rows.Contains(i) == true)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < NumCols; j++)
                    {
                        isFilledCopy[newIndex, j] = IsFilled[i, j];
                        gridBrushesCopy[newIndex, j] = GridBrushes[i, j];
                    }
                    newIndex--;
                }
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < NumCols; j++)
                {
                    isFilledCopy[i, j] = false;
                }
            }

            IsFilled = isFilledCopy;
            GridBrushes = gridBrushesCopy;

            Score += rows.Count();
        }

        public void MoveLeft()
        {
            CurrentFigure.MoveLeft(IsFilled);
        }

        public void MoveRight()
        {
            CurrentFigure.MoveRight(IsFilled);
        }

        public void Rotate()
        {
            CurrentFigure.Rotate(IsFilled);
        }

        public void FastStep()
        {
            while (CheckBottomHit() == false)
            {
                CurrentFigure.MoveDown();
            }
        }

        void PutOnGrid()
        {
            for (int i = 0; i < CurrentFigure.Items.Length; ++i)
            {
                int col_index = CurrentFigure.Items[i].X / Size;
                int row_index = CurrentFigure.Items[i].Y / Size;

                IsFilled[row_index, col_index] = true;
                GridBrushes[row_index, col_index] = CurrentFigure.Brush;
            }
        }

    }
}
