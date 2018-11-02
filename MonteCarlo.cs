using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellularAutomata;
using System.Drawing;
namespace SeedGrowth
{
    class MonteCarlo : Automat2D
    {
        int[,] Id;
        List<Color> colors;
        Random random;


        public MonteCarlo(int N, int M, int numberofseeds) : base(N, M)
        {
            random = new Random(DateTime.Now.Millisecond);
            Id = new int[N, M];
            colors = new List<Color>();
            for (int i = 0; i < numberofseeds; i++)
                colors.Add(Color.FromArgb(255, random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    Id[i, j] = random.Next(0, numberofseeds);
                    Cells[i, j] = colors[Id[i, j]].ToArgb();
                }
        }


        public override int getCellstate(List<int> neighbours, int i, int j)
        {

            int initialEnergy = neighbours.Where(value => value != Cells[i, j]).Count();
            int endEnergy;
            Color newColor = default(Color);
            while (true)
            {

                newColor = colors[random.Next(0, colors.Count)];
                endEnergy = neighbours.Where(value => value != newColor.ToArgb()).Count();
                if (endEnergy <= initialEnergy)
                    break;
            }
            Id[i, j] = colors.IndexOf(newColor);
            return newColor.ToArgb();
            //return Cells[i, j];

        }
    }
}