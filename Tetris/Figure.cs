using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal interface IFigure
    {
        SolidBrush Brush { get; set; }
        Rectangle[] Items { get; set; }
        int Size { get; set; }
        void Step();
        void MoveLeft(bool[,] gameGrid);
        void MoveRight(bool[,] gameGrid);
        void Rotate(bool[,] gameGrid);
        bool CheckCollisions(bool[,] gameGrid);
        void Reset();
    }

    internal abstract class BasicFigure: IFigure
    {
        Rectangle[] items;
        protected int size;
        protected int spawnX;
        SolidBrush brush;

        public Rectangle[] Items { get => this.items; set => this.items = value; }
        public int Size { get => this.size; set => this.size = value; }
        public SolidBrush Brush { get => this.brush; set => this.brush = value; }       

        public bool CheckCollisions(bool[,] gameGrid)
        {
            for (int i = 0; i < this.Items.Length; i++)
            {
                if (this.Items[i].X / this.size > gameGrid.GetLength(1) - 1)
                {
                    return true;
                }

                if (this.Items[i].X < 0)
                {
                    return true;
                }

                if (this.Items[i].Y / this.size > gameGrid.GetLength(0) - 1)
                {
                    return true;
                }

                if (this.Items[i].Y < 0)
                {
                    return true;
                }

                int row_index = this.Items[i].Y / this.size;
                int col_index = this.Items[i].X / this.size;

                if (gameGrid[row_index, col_index] == true)
                {
                    return true;
                }

            }

            return false;
        }

        public void MoveLeft(bool[,] gameGrid)
        {         
            Rectangle[] oldState = new Rectangle[this.Items.Length];
            Array.Copy(this.Items, oldState, oldState.Length);

            for (int i = 0; i < this.Items.Length; i++)
            {
                this.Items[i].X -= this.Size;
            }

            if (this.CheckCollisions(gameGrid) == true)
            {
                this.Items = oldState;
            }
        }

        public void MoveRight(bool[,] gameGrid)
        {
            Rectangle[] oldState = new Rectangle[this.Items.Length];
            Array.Copy(this.Items, oldState, oldState.Length);

            for (int i = 0; i < this.Items.Length; i++)
            {
                this.Items[i].X += this.Size;
            }

            if (this.CheckCollisions(gameGrid) == true)
            {
                this.Items = oldState;
            }
        }

        virtual public void Rotate(bool[,] gameGrid)
        {
            Rectangle[] oldState = new Rectangle[this.Items.Length];
            Array.Copy(this.Items, oldState, oldState.Length);

            for (int i = 0; i < this.Items.Length; i++)
            {
                if (i == 1)
                {
                    continue;
                }

                int tempX = this.Items[i].X;
                int tempY = this.Items[i].Y;

                this.Items[i].X = -(tempY - Items[1].Y) + Items[1].X;
                this.Items[i].Y = +(tempX - Items[1].X) + Items[1].Y;
            }

            if (this.CheckCollisions(gameGrid) == true)
            {
                this.Items = oldState;
            }
        }


        public void Step()
        {
            for (int i = 0; i < this.Items.Length; i++)
            {
                this.Items[i].Y += this.Size;
            }
        }

        public abstract void Reset();

        public BasicFigure(int size, int spawnX)
        {
            this.Items = new Rectangle[4];
            this.Size = size;
            this.spawnX = spawnX;
        }

    }

    internal class LineFigure : BasicFigure
    {
        public LineFigure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        public override void Reset()
        {
            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 2 * size, 0, size, size);
            this.Items[3] = new Rectangle(this.spawnX + 3 * size, 0, size, size);
        }

    }

    internal class SquareFigure : BasicFigure
    {
        public SquareFigure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        override public void Rotate(bool[,] gameGrid)
        {

        }
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 0, size, size, size);
            this.Items[3] = new Rectangle(this.spawnX + size, size, size, size);
        }

    }

    internal class L1Figure : BasicFigure
    {
        public L1Figure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }        
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 2 * size, 0, size, size);
            this.Items[3] = new Rectangle(this.spawnX + 2 * size, size, size, size);
        }

    }

    internal class L2Figure : BasicFigure
    {
        public L2Figure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 2 * size, 0, size, size);
            this.Items[3] = new Rectangle(this.spawnX + 0, size, size, size);
        }

    }

    internal class TFigure : BasicFigure
    {
        public TFigure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 2 * size, 0, size, size);
            this.Items[3] = new Rectangle(this.spawnX + size, size, size, size);
        }

    }

    internal class S1Figure : BasicFigure
    {
        public S1Figure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + 0, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + 0, size, size, size);
            this.Items[2] = new Rectangle(this.spawnX + size, size, size, size);
            this.Items[3] = new Rectangle(this.spawnX + size, 2 * size, size, size);
        }

    }

    internal class S2Figure : BasicFigure
    {
        public S2Figure(int size, int spawnX) : base(size, spawnX)
        {
            Reset();
        }
        public override void Reset()
        {

            this.Items[0] = new Rectangle(this.spawnX + size, 0, size, size);
            this.Items[1] = new Rectangle(this.spawnX + size, size, size, size);
            this.Items[2] = new Rectangle(this.spawnX + 0, size, size, size);
            this.Items[3] = new Rectangle(this.spawnX + 0, 2 * size, size, size);
        }

    }


}
