using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CellularAutomata
{
    enum structure
    {
        Immutable,
        Oscilator,
        Glider,
        Dakota
    }
    enum neigbhourhoodType
    {
        Moore,
        MoorePeriodic,
        VonNoyman,
        VonNoymanPeriodic,
        HexagonalRandom,
        HexagonalRandomPeriodic,
        PentagonalRandom,
        PentagonalRandomPeriodic

    }
    delegate List<int> getneighbours(int i, int j);
    class Automat2D
    {
        private int[,] cells;
        private int ibound;
        private int jbound;
        private volatile bool work = false;
        private getneighbours getneighbours;


        public int[,] Cells
        {

            get
            {
                return cells;
            }

            set
            {
                cells = value;
            }
        }

        public int Ibound
        {
            get
            {
                return ibound;
            }

            set
            {
                ibound = value;
            }
        }

        public int Jbound
        {
            get
            {
                return jbound;
            }

            set
            {
                jbound = value;
            }
        }

        public bool Work
        {
            get
            {
                return work;

            }

            set
            {
                work = value;
            }
        }

        public void stop() { work = false; }

        public void setStructure(structure structure)
        {
            int xmid = ibound / 2;
            int ymid = jbound / 2;

            switch (structure)
            {
                case structure.Immutable:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid - 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 2, ymid + 1] = 1;
                    break;
                case structure.Oscilator:
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    break;
                case structure.Glider:
                    cells[xmid, ymid] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    break;
                case structure.Dakota:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid - 1] = 1;
                    cells[xmid, ymid - 2] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 3, ymid] = 1;
                    cells[xmid + 1, ymid - 3] = 1;
                    cells[xmid + 4, ymid - 1] = 1;
                    cells[xmid + 4, ymid - 3] = 1;
                    break;
            }
        }
        public void setStructure(structure structure, int x, int y)
        {
            int xmid = x;
            int ymid = y;

            switch (structure)
            {
                case structure.Immutable:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid - 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 2, ymid + 1] = 1;
                    break;
                case structure.Oscilator:
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    //cells[xmid, ++ymid] = 1;
                    break;
                case structure.Glider:
                    cells[xmid, ymid] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    break;
                case structure.Dakota:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid - 1] = 1;
                    cells[xmid, ymid - 2] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 3, ymid] = 1;
                    cells[xmid + 1, ymid - 3] = 1;
                    cells[xmid + 4, ymid - 1] = 1;
                    cells[xmid + 4, ymid - 3] = 1;
                    break;
            }
        }

        public Automat2D(int N, int M)
        {
            cells = new int[N, M];

            ibound = cells.GetUpperBound(0);
            jbound = cells.GetUpperBound(1);


        }


        private getneighbours getNeighboursDelegate(neigbhourhoodType type)
        {
            switch (type)
            {
                case neigbhourhoodType.MoorePeriodic:
                    return new getneighbours(getNeighboursMoorePeriodic);
                case neigbhourhoodType.VonNoyman:
                    return new getneighbours(getNeigboursVonNoyman);
                case neigbhourhoodType.VonNoymanPeriodic:
                    return new getneighbours(getNeigboursVonNoymanPeriodic);
                case neigbhourhoodType.Moore:
                    return new getneighbours(getNeighboursMoore);
                case neigbhourhoodType.HexagonalRandom:
                    return new getneighbours(getNeighboursRandomHexagonal);
                case neigbhourhoodType.HexagonalRandomPeriodic:
                    return new getneighbours(getNeighboursRandomHexagonalPeriodic);
                case neigbhourhoodType.PentagonalRandom:
                    return new getneighbours(getNeighboursRandomPentagonal);
                case neigbhourhoodType.PentagonalRandomPeriodic:
                    return new getneighbours(getNeighboursRandomPentagonalPeriodic);

            }
            return null;
        }

        public virtual void Iterate(Action oniterate, neigbhourhoodType neigbhourhoodType)
        {

            getneighbours = getNeighboursDelegate(neigbhourhoodType);
            work = true;
            while (work)
            {

                ComputeNextIterationCells();
                oniterate();
            }
        }


        private void ComputeNextIterationCells()
        {

            int[,] newCells = new int[cells.GetLength(0), cells.GetLength(1)];
            //parallel version
            //Parallel.For(0, ibound + 1, index =>
            //   {
            //       Random random = new Random(DateTime.Now.Millisecond);
            //       for (int j = 0; j < newCells.GetLength(1); j++)
            //       { 
            //           newCells[index,j]  = getCellstate(getneighbours(index, j), index, j);
            //       }
            //   });
            // sequential version
            for (int i = 0; i < newCells.GetLength(0); i++)
                for (int j = 0; j < newCells.GetLength(1); j++)
                    newCells[i, j] = getCellstate(getneighbours(i, j), i, j);

            cells = newCells;
        }

        private List<int> getNeighboursMoorePeriodic(int i, int j)
        {
            List<int> neighbours = new List<int>(8);

            if (i == 0 && j != 0 && j != jbound)
            {

                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[ibound, j + 1]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[i + 1, jbound]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[i - 1, 0]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[i + 1, jbound]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[ibound, j + 1]);

            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[0, jbound]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[i - 1, 0]);

            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[ibound, 0]);

            }
            else
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }


            return neighbours;
        }

        private List<int> getNeigboursVonNoymanPeriodic(int i, int j)
        {
            List<int> neighbours = new List<int>(4);
            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[0, j]);
            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i, jbound]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, 0]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[i, jbound]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[i, 0]);
            }
            else
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeigboursVonNoyman(int i, int j)
        {
            List<int> neighbours = new List<int>();

            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j]);
            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j - 1]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
            }
            else
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeighboursMoore(int i, int j)
        {
            List<int> neighbours = new List<int>();

            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                for (int k = j - 1; k <= j + 1; k++)
                    neighbours.Add(cells[i + 1, k]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {


                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (j == 0 && i != 0 && i != ibound)
            {

                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {

                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);


            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);

            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);


            }
            else
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeighboursRandomPentagonal(int i, int j)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<int> neighbours = getNeighboursMoore(i, j);



            if (i == 0 && j != 0 && j != jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // left Pentagon

                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        break;
                    case 1: // right Pentagon
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);

                        break;
                    case 2: // top Pentagon
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 3: // bottom Pentagon                       
                        break;
                }
            }
            else if (i == ibound && j != 0 && j != jbound)
            {


                switch (random.Next(0, 4))
                {
                    case 0: // lewy

                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);

                        break;
                    case 1: // prawy
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2: //gorny

                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }

            }
            else if (j == 0 && i != 0 && i != ibound)
            {

                switch (random.Next(0, 4))
                {
                    case 0: // lewy

                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        break;
                    case 1: // prawy

                        break;
                    case 2: //gorny
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);

                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(0);

                        break;
                }
            }
            else if (j == jbound && i != 0 && i != ibound)
            {

                switch (random.Next(0, 4))
                {
                    case 0: // lewy


                        break;
                    case 1: // prawy
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2: //gorny
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);

                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);

                        break;
                }
            }
            else if (i == 0 && j == 0)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // lewy

                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);

                        break;
                    case 1: // prawy

                        break;
                    case 2: //gorny
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                    case 3: //dolny

                        break;
                }


            }
            else if (i == ibound && j == 0)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // lewy

                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);

                        break;
                    case 1: // prawy

                        break;
                    case 2: //gorny

                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(0);

                        break;
                }

            }
            else if (i == ibound && j == jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // lewy


                        break;
                    case 1: // prawy
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);

                        break;
                    case 2: //gorny

                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);

                        break;
                }

            }
            else if (i == 0 && j == jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // lewy


                        break;
                    case 1: // prawy
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);

                        break;
                    case 2: //gorny
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);

                        break;
                    case 3: //dolny

                        break;
                }


            }
            else
            {
                switch (random.Next(0, 4))
                {
                    case 0: // lewy

                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(6);
                        neighbours.RemoveAt(5);
                        break;
                    case 1: // prawy
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2: //gorny
                        neighbours.RemoveAt(5);
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        break;
                    case 3: //dolny
                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }
            }

            return neighbours;
        }

        private List<int> getNeighboursRandomPentagonalPeriodic(int i, int j)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<int> neighbours = new List<int>();
            neighbours = getNeighboursMoorePeriodic(i, j);
            switch (random.Next(0, 4))
            {
                case 0: // left side

                    neighbours.RemoveAt(7);
                    neighbours.RemoveAt(6);
                    neighbours.RemoveAt(5);
                    break;
                case 1: // right side
                    neighbours.RemoveAt(1);
                    neighbours.RemoveAt(1);
                    neighbours.RemoveAt(1);
                    break;
                case 2: // top side
                    neighbours.RemoveAt(3);
                    neighbours.RemoveAt(3);
                    neighbours.RemoveAt(3);
                    break;
                case 3: // bottom side
                    neighbours.RemoveAt(0);
                    neighbours.RemoveAt(0);
                    neighbours.RemoveAt(5);
                    break;
            }
            return neighbours;
        }

        private List<int> getNeighboursRandomHexagonal(int i, int j)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<int> neighbours = neighbours = getNeighboursMoore(i, j);

            if (i == 0 && j != 0 && j != jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // top to bottom Hexagon

                        neighbours.RemoveAt(1);

                        break;
                    case 1: // bottom to top Hexagon
                        neighbours.RemoveAt(3);

                        break;

                }
            }
            else if (i == ibound && j != 0 && j != jbound)
            {


                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(4);

                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(1);

                        break;

                }

            }
            else if (j == 0 && i != 0 && i != ibound)
            {

                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(4);

                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(2);

                        break;

                }
            }
            else if (j == jbound && i != 0 && i != ibound)
            {

                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(3);

                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(1);

                        break;

                }
            }
            else if (i == 0 && j == 0)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu



                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(1);

                        break;

                }


            }
            else if (i == ibound && j == 0)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(2);

                        break;
                    case 1: // z dolu do gory


                        break;

                }

            }
            else if (i == ibound && j == jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu



                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(1);

                        break;

                }

            }
            else if (i == 0 && j == jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(1);

                        break;
                    case 1: // z dolu do gory


                        break;

                }


            }
            else
            {
                switch (random.Next(0, 2))
                {
                    case 0: // z gory do dolu

                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(4);

                        break;
                    case 1: // z dolu do gory
                        neighbours.RemoveAt(5);
                        neighbours.RemoveAt(1);

                        break;

                }
            }
            return neighbours;
        }

        private List<int> getNeighboursRandomHexagonalPeriodic(int i, int j)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<int> neighbours = getNeighboursMoorePeriodic(i, j);

            switch (random.Next(0, 2))
            {
                case 0: // top to bottom Hexagon

                    neighbours.RemoveAt(6);
                    neighbours.RemoveAt(3);
                    break;
                case 1: // bottom to top Hexagon
                    neighbours.RemoveAt(4);
                    neighbours.RemoveAt(1);
                    break;

            }
            return neighbours;
        }

        public virtual int getCellstate(List<int> neighbours, int i, int j)
        {
            int count = (from x in neighbours
                         where x != 0
                         select x).Count();
            switch (cells[i, j])
            {
                case 0:
                    return count == 3 ? 1 : 0;
                case 1:
                    if (count == 2 || count == 3)
                        return 1;
                    else if (count > 3 || count < 2)
                        return 0;
                    break;
            }
            return 0;
        }
    }
}