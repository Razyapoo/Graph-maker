using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystAnalys_lr1
{
    class Vrchol
    {
        public int x, y;

        public Vrchol(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Hrana
    {
        public int v1, v2, v3;
        public bool v4;

        public Hrana(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = 0;
            this.v4 = false;
        }
        public Hrana(int v1, int v2, int v3, bool v4)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
        }
    }

    class DrawGraph
    {
        Bitmap bitmap;
        Pen blackPen;
        Pen redPen;
        Pen darkGoldPen;
        Graphics gr;
        Font fo = new Font("Arial", 10);
        Brush br;
        PointF point;
        public int R = 11; //poloměr koule vrcholu

        public DrawGraph(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            darkGoldPen = new Pen(Color.DarkGoldenrod);
            darkGoldPen.Width = 2;
            br = Brushes.Black;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }

        public void drawVrchol(int x, int y, string number)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            int size = (int)(number.Length * fo.SizeInPoints / 2.3);
            point = new PointF(x - size, y - 8);
            gr.DrawString(number, fo, br, point);
        }

        public void drawSelectedVrchol(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void drawHrana(Vrchol V1, Vrchol V2, Hrana E, int numberE)
        {
            if (E.v1 == E.v2)
            {
                gr.DrawArc(darkGoldPen, (V1.x - 2 * R), (V1.y - 2 * R), 2 * R, 2 * R, 90, 270);
                point = new PointF(V1.x - (int)(2.75 * R), V1.y - (int)(2.75 * R));
                gr.DrawString(E.v3.ToString(), fo, br, point);
                drawVrchol(V1.x, V1.y, (E.v1 + 1).ToString());
            }
            else
            {
                gr.DrawLine(darkGoldPen, V1.x, V1.y, V2.x, V2.y);
                point = new PointF((V1.x + V2.x) / 2, (V1.y + V2.y) / 2);
                gr.DrawString(E.v3.ToString(), fo, br, point);
                drawVrchol(V1.x, V1.y, (E.v1 + 1).ToString());
                drawVrchol(V2.x, V2.y, (E.v2 + 1).ToString());
            }
        }

        public void drawALLGraph(List<Vrchol> V, List<Hrana> E)
        {
            //kreslíme hrany
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].v1 == E[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (V[E[i].v1].x - 2 * R), (V[E[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    point = new PointF(V[E[i].v1].x - (int)(2.75 * R), V[E[i].v1].y - (int)(2.75 * R));
                    gr.DrawString(E[i].v3.ToString(), fo, br, point);
                }
                else
                {
                    gr.DrawLine(darkGoldPen, V[E[i].v1].x, V[E[i].v1].y, V[E[i].v2].x, V[E[i].v2].y);
                    point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2, (V[E[i].v1].y + V[E[i].v2].y) / 2);
                    gr.DrawString(E[i].v3.ToString(), fo, br, point);
                }
            }
            //kreslíme vrcholy
            for (int i = 0; i < V.Count; i++)
            {
                drawVrchol(V[i].x, V[i].y, (i + 1).ToString());
            }
        }

        //naplníme matici sousednosti
        public void fillAdjacencyMatrix(int numberV, List<Hrana> E, int[,] matrix)
        {
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].v3 > 0) matrix[E[i].v1, E[i].v2] = E[i].v3; else matrix[E[i].v1, E[i].v2] = 1;
                if (!E[i].v4) matrix[E[i].v2, E[i].v1] = matrix[E[i].v1, E[i].v2];
            }
        }
       
    }
}