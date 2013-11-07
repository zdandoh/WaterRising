using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    public class UI
    {
        static char[,] map = new char[15,25];
        static int[] map_start = { 1, 53 };
        static int[] log_coords = { 1, 1 };
        static bool map_exists = false;
        static StringBuilder frame = new StringBuilder(@"
|---------------------------------------------------|-------------------------|
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |-------------------------|
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|---------------------------------------------------|                         |
|                                                   |                         |
|---------------------------------------------------|-------------------------|
");
        static StringBuilder blank_frame = new StringBuilder(frame.ToString());
        public static void Update()
        {
            Console.Clear();
            Console.Write(frame.ToString());
            if (map_exists == true)
            {
                DrawMap();
            }
            Console.CursorLeft = 1;
            Console.CursorTop = 22;
        }

        public static void UpdateMap(byte[,] planet, int[] player)
        {
            map_exists = true;
            int[] top_left = { player[0] - 8, player[1] - 13};
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 25; col++)
                {
                    // iterate through every map byte
                    char map_raw = planet[top_left[0] + row, top_left[1] + col].ToString().Single();
                    map[row, col] = map_raw;
                    frame[(map_start[0] + row) * 81 + (map_start[1] + col) + 2] = map_raw;
                }
            }
            Update();
        }

        public static void DrawMap()
        {
            Console.CursorVisible = false;
            Console.CursorLeft = 53;
            Console.CursorTop = 2;
            for (int row = 0; row < 15; row++ )
            {
                for (int col = 0; col < 25; col++)
                {
                    char map_raw = map[row, col];
                    Console.ResetColor();
                    if (map_raw == '0')
                    {
                        // GROUND
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        map_raw = ' ';
                    }
                    else if (map_raw == '1')
                    {
                        // TREE
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        map_raw = '^';
                    }
                    else if (map_raw == '2')
                    {
                        // WATER
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        map_raw = '~';
                    }
                    else if (map_raw == '3')
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Red;
                        map_raw = '#';
                    }
                    else
                    {
                        System.ArgumentException bad_block = new System.ArgumentException(String.Format("Block with id {0} does not exist", map_raw));
                        throw bad_block;
                    }
                    Console.Write(map_raw);
                }
                if (Console.CursorTop < 24)
                {
                    Console.CursorTop++;
                }
                Console.CursorLeft = 53;
            }
            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public static void Log(string str)
        {
            char[] char_array = str.ToCharArray();
            foreach (char c in char_array)
            {
                if (log_coords[1] > 50)
                {
                    log_coords[1] = 1 + log_coords[0];
                    log_coords[0]++;
                }
                if (log_coords[0] >= 21)
                {
                    Clear();
                }
                frame[((log_coords[0] * 80) + log_coords[1] + 4)] = c;
                log_coords[1]++;
            }
            log_coords[0]++;
            log_coords[1] = log_coords[0];
            Update();
        }

        public static string ReadLine()
        {
            Console.CursorVisible = true;
            string console_output = Console.ReadLine();
            Console.CursorVisible = false;
            Update();
            return console_output;
        }

        public static void Clear()
        {
            // Clear log chars
            for (int clear_count = 0; clear_count <= 12; clear_count++)
            {
                Console.Clear();
                frame = new StringBuilder(blank_frame.ToString());
                UpdateMap(Program.PlanetGen.GetPlanet(), Program.player);
                log_coords[0] = 1;
                log_coords[1] = 1;
            }
        }
    }
}