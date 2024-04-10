using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form : System.Windows.Forms.Form
    {
        Scene scene;
        public Form()
        {
            InitializeComponent();
            int width = 440;
            int height = 600;
            int itemSize = 40;

            this.Size = new Size(width + itemSize/2, height + itemSize);



            this.scene = new Scene(this, height, width, itemSize);        
            this.Paint += new PaintEventHandler(GameGridPaint);
            this.Paint += new PaintEventHandler(FigurePaint);

            this.KeyDown += KeyDownHalder;
        }

        private void KeyDownHalder(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                this.scene.MoveLeft();
            }

            if (e.KeyCode == Keys.D)
            {
                this.scene.MoveRight();
            }
            if (e.KeyCode == Keys.W)
            {
                this.scene.Rotate();
            }
            if (e.KeyCode == Keys.S)
            {
                this.scene.FastStep();
            }

            this.Invalidate();

        }
        private void GameGridPaint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            pen.Width = 2;
            SolidBrush brush;        

            for (int i = 0; i < this.scene.num_rows; i++)
            {
                for (int j = 0; j < this.scene.numCols; j++)
                {

                    if (this.scene.isFilled[i, j] == true)
                    {
                        brush = this.scene.gridBrushes[i, j];
                        g.DrawRectangle(pen, this.scene.gameGrid[i, j]);
                        g.FillRectangle(brush, this.scene.gameGrid[i, j]);
                    }
                }
            }

            pen.Dispose();
        }

        private void FigurePaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            pen.Width = 2;

            SolidBrush brush = this.scene.currentFigure.Brush;

            for (int i = 0; i < this.scene.currentFigure.Items.Length; i++)
            {
                g.DrawRectangle(pen, this.scene.currentFigure.Items[i]);
                g.FillRectangle(brush, this.scene.currentFigure.Items[i]);
            }

            pen.Dispose();
        }

        private void Form_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.scene.Step();
            this.Invalidate();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
