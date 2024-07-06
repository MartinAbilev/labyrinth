using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace OglShel
{
    class Program
    {
        static DeepLab lab;

        public static void ThreadProc()
        {
            OglshelObject Ogl = new OglshelObject(lab,"Deep OGL SHEL V1.0");
        }
        static void Main(string[] args)
        {
            
            using (lab = new DeepLab())
            {
                
            }
            
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();

            Console.WriteLine("OGL redy");

            
        }
       
    }


    public class DeepLab : IDisposable 
    {
      
        

        public class Cell
        {
            Point pos;

            public int depth;

            public int X
            {
                get { return pos.X; }
                set { pos.X = value; }
            }

            public int Y
            {
                get { return pos.Y; }
                set { pos.Y = value; }
            }

            public Cell(int x, int y,int Depth)
            {
                pos.X = x;
                pos.Y = y;
                depth=Depth;

            }
        }


        public class Path : Math
        {
            Point start;
            Point end;
            int lenght;
            int depthLevel;
            int x;
            int y;


            public Cell[] cells;

            public Cell[] lab;

            public Path(Cell[] Lab, Point Start, int DepthLevel,int Lenght)
            {
                x = Start.X;
                y = Start.Y;

                int Right = 0;
                int Left = 1;
                int Up = 2;
                int Down = 3;

                int Dir;

                //cells = new Cell[Lenght];

                lab = Lab;

                createCell(Start, 0, DepthLevel);// pirma shuna
                
                for(int i=1; i <Lenght; i++)// taisam tik shunas cik ir dotais garums
                {
                    Dir = rnd(0, 4);
                    int newX = 0;
                    int newY = 0;

                    Point newPos=new Point();

                    if (Dir == Right)
                    {
                        newX = x + 1;
                        newY = y;

                        //sheit parbaudam vai jauna vieta ir briva


                        //ja ir briva taisam jaunu shunu
                        newPos.X=newX;
                        newPos.Y=newY;



                        if (empty(newPos))// vai ir tuksh jaunaja pozicija
                            if (empty(new Point(newPos.X+1, newPos.Y )))
                                if (empty(new Point(newPos.X , newPos.Y + 1)))
                                    if (empty(new Point(newPos.X, newPos.Y - 1))) createCell(newPos, i, DepthLevel);// un vai nakama ari tuksha
                                    else Dir = Left;

                    }


                    if (Dir == Left)
                    {
                        newX = x - 1;
                        newY = y;

                        //sheit parbaudam vai jauna vieta ir briva


                        //ja ir briva taisam jaunu shunu
                        newPos.X = newX;
                        newPos.Y = newY;



                        if (empty(newPos))// vai ir tuksh jaunaja pozicija
                            if (empty(new Point(newPos.X -1, newPos.Y)))
                                if (empty(new Point(newPos.X , newPos.Y + 1)))
                                    if (empty(new Point(newPos.X, newPos.Y - 1))) createCell(newPos, i, DepthLevel);// un vai nakama ari tuksha
                                    else Dir = Up;

                    }


                    if (Dir == Up)
                    {
                        newY = y + 1;
                        newX = x;

                        //sheit parbaudam vai jauna vieta ir briva


                        //ja ir briva taisam jaunu shunu
                        newPos.X = newX;
                        newPos.Y = newY;



                        if (empty(newPos))// vai ir tuksh jaunaja pozicija
                            if (empty(new Point(newPos.X , newPos.Y+ 1)))
                                if (empty(new Point(newPos.X-1, newPos.Y )))
                                    if (empty(new Point(newPos.X + 1, newPos.Y))) createCell(newPos, i, DepthLevel);// un vai nakama ari tuksha
                                    else Dir = Down;

                    }



                    if (Dir == Down)
                    {
                        newY = y - 1;
                        newX = x;

                        //sheit parbaudam vai jauna vieta ir briva


                        //ja ir briva taisam jaunu shunu
                        newPos.X = newX;
                        newPos.Y = newY;



                        if (empty(newPos))// vai ir tuksh jaunaja pozicija
                            if (empty(new Point(newPos.X, newPos.Y - 1)))
                                if (empty(new Point(newPos.X - 1, newPos.Y )))
                                    if (empty(new Point(newPos.X + 1, newPos.Y))) createCell(newPos, i, DepthLevel);// un vai nakama ari tuksha

                    }

                    


                    



                }

            }


            bool empty(Point pos)
            {
                for (int i = 0; i < lab.Length; i++)
                {
                    if (lab[i].X == pos.X && lab[i].Y == pos.Y)
                        return false;


                }

                return true;
            }

            void createCell(Point pos,int Id,int Depth)
            {

                if (cells == null) cells = new Cell[1];
                else Array.Resize(ref cells, cells.Length + 1);
                cells[cells.Length-1] = new Cell(pos.X, pos.Y,Depth);// ierakstam kurenta celja shunu araiza


                // sheit jabut funkcijai kas ari globalaja shunu araiza ieraksta lai citi atzari ari redz
                if (lab == null) lab = new Cell[1];
                else
                Array.Resize(ref lab, lab.Length + 1);

                lab[lab.Length - 1] = cells[cells.Length - 1];

                x = pos.X; y = pos.Y;

            }

        }

        int depth;
        int lenght;
        int branching;

        Point PrimStart;
        Point PrimEnd;


        Path PrimPath;

        Path[] AllPath;

        public Cell[] Lab;//  all  labyrynth together

        Math m = new Math();

        public DeepLab()
        {
            depth = 4;
            lenght = 320;
            branching = 4;

            PrimStart.X = 0;
            PrimStart.Y = 0;

            createPrimPath(PrimStart, lenght);
           createSecPath(lenght);
        }

        void IDisposable.Dispose()
        {
            Console.WriteLine("Disposing limited resource.");
        }

        



        void createPrimPath(Point Start,int Lenght)
        {
            PrimPath = new Path(Lab, Start, 0, Lenght);
            Lab = PrimPath.lab;// ierakstam  visas jaunas shunas kopeja

            if (AllPath == null) AllPath = new Path[1];
            else Array.Resize(ref AllPath, AllPath.Length + 1);

            AllPath[AllPath.Length - 1] = PrimPath;// ierakstam pirmo celju kopejaa celju arayza
        }

        void createSecPath(int Lenght)
        {
            for (int i = 0; i < depth; i++)// tik cik dziljuma
            {
                for (int n = 0; n < branching; n++)// tikcik sazarojas
                {
                    int startCelID = m.rnd(0, AllPath[i].cells.Length-1);

                    Point startCell=new Point();

                    startCell.X = AllPath[i].cells[startCelID].X;

                    startCell.Y = AllPath[i].cells[startCelID].Y;

                    //if (AllPath == null) AllPath = new Path[1];
                        //else
                        
                        Array.Resize(ref AllPath, AllPath.Length + 1);

                    AllPath[AllPath.Length - 1] = new Path(Lab, startCell, i + 1, Lenght);
                    Lab = AllPath[AllPath.Length - 1].lab;

                }

            }



        }



    }

    public class Math
    {
        static Random random = new Random();


        public int rnd(int min, int max)
        {
            return random.Next(min, max);

        }


    }
}
