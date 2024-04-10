using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Scene
    {
        public Rectangle[,] gameGrid;
        public bool[,] isFilled;
        public SolidBrush[,] gridBrushes;
        public IFigure currentFigure;
        Form parentForm;
        public int num_rows, numCols;
        public int height, width, size;

        Random random;
        BasicFigure[] figures;
        SolidBrush[] figureBrushes = new SolidBrush[] { 
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


        public Scene(Form form, int height, int width, int size) 
        {
            this.height = height;
            this.width = width;
            this.size = size;

            this.num_rows = height / size;
            this.numCols = width / size;

            this.gameGrid = new Rectangle[num_rows, numCols];
            this.isFilled = new bool[num_rows, numCols];
            this.parentForm = form;

            this.gridBrushes = new SolidBrush[num_rows, numCols];

            for (int i = 0; i < num_rows; i++) 
            { 
                for (int j = 0; j < numCols; j++)
                {
                    this.isFilled[i, j] = false;
                    this.gameGrid[i, j] = new Rectangle(j*size, i * size, size, size);
                }
            }

            this.figures = new BasicFigure[] { 
                                                new SquareFigure(size, numCols/2*size), 
                                                new LineFigure(size, numCols/2*size), 
                                                new L1Figure(size, numCols / 2 * size), 
                                                new L2Figure(size, numCols / 2 * size),
                                                new TFigure(size, numCols / 2 * size),
                                                new S1Figure(size, numCols/2*size),
                                                new S2Figure(size, numCols/2*size),
                                                };

            this.random = new Random();
            this.currentFigure = figures[this.random.Next(figures.Length)];
            this.currentFigure.Brush = this.figureBrushes[this.random.Next(figureBrushes.Length)];

        }

        bool CheckBottomHit()
        {
            for (int i = 0; i < currentFigure.Items.Length; i++)
            {
                if (currentFigure.Items[i].Y + this.size >= this.height) 
                {
                    return true;
                }

                int row_index = currentFigure.Items[i].Y / this.size;
                int col_index = currentFigure.Items[i].X / this.size;

                if (this.isFilled[row_index + 1, col_index] == true)
                {
                    return true;
                }

            }

            return false;
        }
        
        public void Step()
        {

            if (this.CheckBottomHit() == true)
            {
                PutOnGrid();
                this.currentFigure = this.figures[this.random.Next(this.figures.Length)];
                this.currentFigure.Brush = this.figureBrushes[this.random.Next(figureBrushes.Length)];
                this.currentFigure.Reset();
                CheckGrid();
            }
            else
            {
                this.currentFigure.Step();
            }
        }


        void CheckGrid()
        {
            List<int> selectedRows = new List<int>();

            for (int i = this.num_rows - 1; i >= 0; i--)
            {
                bool flag = true;

                for (int j = 0; j < this.numCols; j++)
                {
                    if (this.isFilled[i, j] == false)
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

            if (selectedRows.Count == 0) 
            {
                return;
            }

            bool[, ] isFilledCopy = new bool[num_rows, numCols];
            SolidBrush[,] gridBrushesCopy = new SolidBrush[num_rows, numCols];

            int newIndex = num_rows - 1;

            for (int i = this.num_rows - 1; i >= 0; i--)
            {
                if (selectedRows.Contains(i) == true)
                {
                    continue;
                }
                else 
                { 
                    for (int j = 0;  j < numCols; j++) 
                    {
                        isFilledCopy[newIndex, j] = this.isFilled[i, j];
                        gridBrushesCopy[newIndex, j] = this.gridBrushes[i, j];
                    }
                    newIndex--;
                }
            }

            for (int i = 0; i < selectedRows.Count; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    isFilledCopy[i, j] = false;
                }
            }

            this.isFilled = isFilledCopy;
            this.gridBrushes = gridBrushesCopy;

        }

        public void MoveLeft()
        {
            this.currentFigure.MoveLeft(this.isFilled);
        }

        public void MoveRight()
        {
            this.currentFigure.MoveRight(this.isFilled);
        }

        public void Rotate()
        {
            this.currentFigure.Rotate(this.isFilled);
        }

        public void FastStep()
        {
            while (this.CheckBottomHit() == false)
            {
                this.currentFigure.Step();
            }
        }

        void PutOnGrid()
        {
            for (int i = 0; i < currentFigure.Items.Length; ++i) 
            {
                int col_index = currentFigure.Items[i].X / this.size;
                int row_index = currentFigure.Items[i].Y / this.size;

                isFilled[row_index, col_index] = true;
                this.gridBrushes[row_index, col_index] = currentFigure.Brush;
            }
        }

    }
}
