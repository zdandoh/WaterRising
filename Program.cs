using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Program
    {
        static Random RandGen = new Random();
        static Stopwatch watch = new Stopwatch();
        static PlanetGenerator PlanetGen = new PlanetGenerator();
        static UI ui = new UI();
        static void Main(string[] args)
        {
            // Setup adventure, yo
            const string VERSION = "0.1";
            const bool DEBUG = true;
            const bool SETUP = false;
            int[] player = { 500, 500 };
            Console.BufferHeight = 25;
            Console.CursorVisible = false;
            if (DEBUG == true)
            {
                Console.CursorVisible = true;
            }
            if (SETUP == true)
            {
                Console.WriteLine("CMDRPG v{0}", VERSION);
                SlowWrite("What's your name? ");
                string name = Console.ReadLine();
                SlowWrite(String.Format("Okay, so your name is {0}? Great!", name));
                SlowWrite("You're going to explore a new planet, what is this planet's name? ");
                string planet = Console.ReadLine();
                SlowWrite(String.Format("Wow, {0} sounds exciting!", planet));
                SlowWrite("Looks like the ship is ready to go, press enter to embark!");
                Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine("Traveling to planet...");
            byte[,] world = PlanetGen.MakePlanet();
            Console.Clear();
            UI.UpdateMap(world, player);
            UI.Log("You've arrived on the planet");
            UI.Update();
            Console.ReadLine();
        }

        static void SlowWrite(string str)
        {
            int place = 0;
            int str_length = str.Length;
            for (int i = 0; i < str_length; i++)
            {
                Console.Write(str[place]);
                System.Threading.Thread.Sleep(30);
                place++;
            }
            Console.Write("\n");
        }
    }
}
