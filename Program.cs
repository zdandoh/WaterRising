using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WaterRising
{
    public class Program
    {
        static Random RandGen = new Random();
        static Stopwatch watch = new Stopwatch();
        public static Stopwatch TickTimer = new Stopwatch();
        public static WorldGenerator PlanetGen = new WorldGenerator();
        static UI ui = new UI();
        public static byte player_tile = 0;
        public static byte[,] world = new byte[1000, 1000];
        static void Main(string[] args)
        {
            // Setup adventure, yo
            const string VERSION = "0.1a";
            const bool DEV = true;
            Console.BufferHeight = 25;
            Console.CursorVisible = false;
            TickTimer.Start();
            Console.Title = String.Format("Water Rising v{0}", VERSION);
            if (DEV == false)
            {
                Console.WriteLine("Water Rising v{0}", VERSION);
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
            world = PlanetGen.MakePlanet();
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

        public static void Log(string info)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt", true);
            file.WriteLine(info);
            file.Close();
        }

        public static byte[][] GetJaggedArray(byte[,] mult_array)
        {
            byte[][] jag_arr = new byte[1000][];
            for (int col = 0; col < 1000; col++)
            {
                jag_arr[col] = new byte[1000];
                for (int row = 0; row < 1000; row++)
                {
                    jag_arr[col][row] = mult_array[col, row];
                }
            }
            return jag_arr;
        }

        public static byte[,] GetMultiArray(byte[][] jag_arr)
        {
            byte[,] multi_arr = new byte[1000, 1000];
            for (int col = 0; col < 1000; col++)
            {
                for (int row = 0; row < 1000; row++)
                {
                    multi_arr[col, row] = jag_arr[col][row];
                }
            }
            return multi_arr;
        } 


        public static void Save()
        {
            // Save world by converting to byte[][]
            byte[][] double_world = GetJaggedArray(world);
            var save_file = new System.Xml.Serialization.XmlSerializer(typeof(byte[][]));
            using (FileStream stream = new FileStream(@"world.xml", FileMode.OpenOrCreate))
            {
                save_file.Serialize(stream, double_world);
            }
        }

        public static void Load()
        {
            // Load world and convert back to byte[,]
            var save_file = new System.Xml.Serialization.XmlSerializer(typeof(byte[][]));
            using (FileStream stream = new FileStream(@"world.xml", FileMode.OpenOrCreate))
            {
                byte[][] load_world = save_file.Deserialize(stream) as byte[][];
                world = GetMultiArray(load_world);
            }
        }

        public static void PlayGame()
        {
            while (true)
            {
                string command = UI.ReadLine();
                if (command.Length >= 1)
                {
                    Player.HandleInput(command);
                }
            }
        }
    }
}
