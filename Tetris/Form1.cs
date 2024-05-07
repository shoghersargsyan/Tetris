using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form : System.Windows.Forms.Form
    {
        readonly Scene scene;
        public Form()
        {
            InitializeComponent();

            int width = 440;
            int height = 600;
            int itemSize = 40;

            BackColor = SystemColors.ActiveCaption;
            Size = new Size(width + itemSize / 2, height + itemSize);
            startGameButton.Location = new Point((width - startGameButton.Width) / 2, 200);
            scene = new Scene(height, width, itemSize);    
            KeyDown += KeyDownHalder;
        }

        private void KeyDownHalder(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                scene.IsPaused = !scene.IsPaused;
            }

            if (scene.IsPaused)
            {
                return;
            }

            if (e.KeyCode == Keys.A)
            {
                scene.MoveLeft();
            }

            if (e.KeyCode == Keys.D)
            {
                scene.MoveRight();
            }
            if (e.KeyCode == Keys.W)
            {
                scene.Rotate();
            }
            if (e.KeyCode == Keys.S)
            {
                scene.FastStep();
            }

            Invalidate();
        }
        private void GameGridPaint(object sender, PaintEventArgs e)
        {

            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Black){Width = 2};
            SolidBrush brush;        

            for (int i = 0; i < scene.NumRows; i++)
            {
                for (int j = 0; j < scene.NumCols; j++)
                {

                    if (scene.IsFilled[i, j] == true)
                    {
                        brush = scene.GridBrushes[i, j];
                        graphics.DrawRectangle(pen, scene.GameGrid[i, j]);
                        graphics.FillRectangle(brush, scene.GameGrid[i, j]);
                    }
                }
            }

            pen.Dispose();
        }

        private void FigurePaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Black){Width = 2};

            SolidBrush brush = scene.CurrentFigure.Brush;

            for (int i = 0; i < scene.CurrentFigure.Items.Length; i++)
            {
                graphics.DrawRectangle(pen, scene.CurrentFigure.Items[i]);
                graphics.FillRectangle(brush, scene.CurrentFigure.Items[i]);
            }

            pen.Dispose();
        } 

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!scene.IsPaused)
            {
                scene.Step();
                Invalidate();
                scoreTextBox.Text = $"Score: {scene.Score}";
            }
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            Paint += new PaintEventHandler(GameGridPaint);
            Paint += new PaintEventHandler(FigurePaint);            

            startGameButton.Visible = false;
            scene.IsPaused = false;
            Focus();
            scoreTextBox.Visible = true;            
        }
    }
}
