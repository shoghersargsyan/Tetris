using System;
using System.Drawing;

namespace Tetris
{
    internal interface IFigure
    {
        SolidBrush Brush { get; set; }
        int Size { get; }
        Rectangle[] Items { get;}
        void MoveDown();
        void MoveLeft(bool[,] gameGrid);
        void MoveRight(bool[,] gameGrid);
        void Rotate(bool[,] gameGrid);
        bool CheckCollisions(bool[,] gameGrid);
        void Reset();
    }

    internal abstract class BasicFigure: IFigure
    {
        public SolidBrush Brush { get; set; }
        public Rectangle[] Items { get;  private set; }
        public int Size { get; private set; }
        protected readonly int spawnX;

        public bool CheckCollisions(bool[,] gameGrid)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].X / Size > gameGrid.GetLength(1) - 1)
                {
                    return true;
                }

                if (Items[i].X < 0)
                {
                    return true;
                }

                if (Items[i].Y / Size > gameGrid.GetLength(0) - 1)
                {
                    return true;
                }

                if (Items[i].Y < 0)
                {
                    return true;
                }

                int rowIndex = Items[i].Y / Size;
                int colIndex = Items[i].X / Size;

                if (gameGrid[rowIndex, colIndex] == true)
                {
                    return true;
                }

            }

            return false;
        }

        public void MoveLeft(bool[,] gameGrid)
        {         
            Rectangle[] oldState = new Rectangle[Items.Length];
            Array.Copy(Items, oldState, oldState.Length);

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].X -= Size;
            }

            if (CheckCollisions(gameGrid) == true)
            {
                Items = oldState;
            }
        }

        public void MoveRight(bool[,] gameGrid)
        {
            Rectangle[] oldState = new Rectangle[Items.Length];
            Array.Copy(Items, oldState, oldState.Length);

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].X += Size;
            }

            if (CheckCollisions(gameGrid) == true)
            {
                Items = oldState;
            }
        }

        virtual public void Rotate(bool[,] gameGrid)
        {
            Rectangle[] oldState = new Rectangle[Items.Length];
            Array.Copy(Items, oldState, oldState.Length);

            for (int i = 0; i < Items.Length; i++)
            {
                if (i == 1)
                {
                    continue;
                }

                int tempX = Items[i].X;
                int tempY = Items[i].Y;

                Items[i].X = -(tempY - Items[1].Y) + Items[1].X;
                Items[i].Y = +(tempX - Items[1].X) + Items[1].Y;
            }

            if (CheckCollisions(gameGrid) == true)
            {
                Items = oldState;
            }
        }


        public void MoveDown()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Y += Size;
            }
        }

        public abstract void Reset();

        public BasicFigure(int size, int spawnX)
        {
            Items = new Rectangle[4];
            Size = size;
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
            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[2] = new Rectangle(spawnX + 2 * Size, 0, Size, Size);
            Items[3] = new Rectangle(spawnX + 3 * Size, 0, Size, Size);
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

            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[2] = new Rectangle(spawnX + 0, Size, Size, Size);
            Items[3] = new Rectangle(spawnX + Size, Size, Size, Size);
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

            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[2] = new Rectangle(spawnX + 2 * Size, 0, Size, Size);
            Items[3] = new Rectangle(spawnX + 2 * Size, Size, Size, Size);
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

            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[2] = new Rectangle(spawnX + 2 * Size, 0, Size, Size);
            Items[3] = new Rectangle(spawnX + 0, Size, Size, Size);
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

            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[2] = new Rectangle(spawnX + 2 * Size, 0, Size, Size);
            Items[3] = new Rectangle(spawnX + Size, Size, Size, Size);
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

            Items[0] = new Rectangle(spawnX + 0, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + 0, Size, Size, Size);
            Items[2] = new Rectangle(spawnX + Size, Size, Size, Size);
            Items[3] = new Rectangle(spawnX + Size, 2 * Size, Size, Size);
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

            Items[0] = new Rectangle(spawnX + Size, 0, Size, Size);
            Items[1] = new Rectangle(spawnX + Size, Size, Size, Size);
            Items[2] = new Rectangle(spawnX + 0, Size, Size, Size);
            Items[3] = new Rectangle(spawnX + 0, 2 * Size, Size, Size);
        }

    }

}
