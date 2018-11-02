using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;
using System.Drawing;

namespace SeedGrowth
{
    class SeedGrowth : Automat2D
    {

        private List<Color> colors = new List<Color>() { Color.FromKnownColor(KnownColor.Coral) };
        Random random = new Random(DateTime.Now.Millisecond);
        public SeedGrowth(int N, int M) : base(N, M)
        {
            for (int i = 0; i <= Ibound; i++)
                for (int j = 0; j <= Jbound; j++)
                    Cells[i, j] = Color.Black.ToArgb();
        }

        public override int getCellstate(List<int> neighbours, int i, int j)
        {
            int aliveneighbours = neighbours.Count(color => color != Color.Black.ToArgb());
            if (Cells[i, j] == Color.Black.ToArgb() && aliveneighbours != 0)
            {

                int dominantColorcount = 0;
                Color dominantColor = Color.FromArgb(Cells[i, j]);
                for (int k = 0; k < colors.Count; k++)
                {
                    int colorcount = neighbours.Count(color => color == colors[k].ToArgb());
                    if (dominantColorcount < colorcount)
                    {
                        dominantColor = colors[k];
                        dominantColorcount = colorcount;
                    }
                }
                return dominantColor.ToArgb();
            }
            else
                return Cells[i, j];
        }
        public void setSeed(int x, int y)
        {

            Color color = Color.FromArgb(255, random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
            colors.Add(color);
            Cells[x, y] = colors[colors.Count - 1].ToArgb();

        }

        public void setSeedsEvenly(int XaxisSeeds, int YaxisSeeds)
        {
            int dx = Convert.ToInt32(Math.Round(Ibound / ((double)(XaxisSeeds))));
            int dy = Convert.ToInt32(Math.Round(Jbound / ((double)(YaxisSeeds))));

            int tmpdx;
            int tmpdy;


            for (int i = 1; i <= XaxisSeeds; i++)
            {
                if (i == 1)
                    tmpdx = Convert.ToInt32(dx/2.0);
                else if (i == XaxisSeeds)
                    tmpdx = Convert.ToInt32(dx*XaxisSeeds-(dx/2.0));
                else
                    tmpdx = Convert.ToInt32(dx*i-(dx/2.0));
                for (int j = 1; j <= YaxisSeeds; j++)
                {
                    if (j == 1)
                        tmpdy = Convert.ToInt32(dy/2.0);
                    else if (j == YaxisSeeds)
                        tmpdy = Convert.ToInt32(dy*YaxisSeeds-(dy/2.0));
                    else
                        tmpdy = Convert.ToInt32(dy*j-(dy/2.0));

                    setSeed(tmpdx, tmpdy);
                }
            }

        }

        public void setSeedsRandomly(int NumberofSeeds)
        {
            // Random random = new Random(DateTime.Now.Second);

            for (int i = 0; i < NumberofSeeds; i++)
            {
                setSeed(random.Next(0, Ibound), random.Next(0, Jbound));
            }
        }

        public void setSeedswithRadius(int NumberofSeeds, int Radius)
        {

            if (Math.Ceiling((Ibound + Jbound) / (double)Radius) < NumberofSeeds)
                throw new Exception("Decrease radius or number of seeds");

            Random random = new Random(DateTime.Now.Second);
            List<Point> seeds = new List<Point>();
            seeds.Add(new Point(random.Next(0, Ibound), random.Next(0, Jbound)));

            int i = 0;
            while (i < NumberofSeeds - 1)
            {
                bool badseed = false;
                Point tmp = new Point(random.Next(0, Ibound), random.Next(0, Jbound));
                foreach (Point p in seeds)
                    if (Radius > getSeedsDistance(p, tmp))
                        badseed = true;
                if (!badseed) { seeds.Add(tmp); i++; }

            }

            foreach (Point p in seeds)
                setSeed(p.X, p.Y);
        }

        private double getSeedsDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
    }
}