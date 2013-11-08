using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    public class Program
    {
        static Random RandGen = new Random();
        static Stopwatch watch = new Stopwatch();
        public static PlanetGenerator PlanetGen = new PlanetGenerator();
        static UI ui = new UI();
        public static byte player_tile = 0;
        static void Main(string[] args)
        {
            // Setup adventure, yo
            const string VERSION = "0.1a";
            const bool DEV = true;
            Console.BufferHeight = 25;
            Console.CursorVisible = false;
            Console.Title = String.Format("Water Rising v{0}", VERSION);
            if (DEV == false)
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
            UI.Update();
            byte[,] world = PlanetGen.MakePlanet();
            UI.UpdateMap(world, Player.pos);
            UI.Log("Arrived at planet!");

            // Main game loop begins
            PlayGame();
        }

        public static void SlowWrite(string str)
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

        public static void PlayGame()
        {
            while (true)
            {
                UI.ReadLine();
            }
        }
    }
}
