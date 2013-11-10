using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WaterRising
{
    public class UI
    {
        static char[,] map = new char[15,25];
        static int[] map_start = { 1, 53 };
        static int[] log_coords = { 1, 1 };
        static bool map_exists = false;
        static StringBuilder frame = new StringBuilder(@"|---------------------------------------------------|-------------------------|
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |-------------------------|
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|---------------------------------------------------|                         |
|                                                   |                         |
|---------------------------------------------------|-------------------------|
");
        static string[] log_data = new string[20];
        static StringBuilder blank_frame = new StringBuilder(frame.ToString());
        static StringBuilder blank_log = new StringBuilder(log_data.ToString());
        public static void Update()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write(frame.ToString());
            for (byte log_entry = 0; log_entry < log_data.Length; log_entry++ )
            {
                Console.CursorTop = log_entry;
                Console.CursorLeft = 1;
                Console.Write(log_data[log_entry]);
            }
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
                    // frame[(map_start[0] + row) * 81 + (map_start[1] + col) + 2] = map_raw;
                }
            }
            Update();
        }

        public static void DrawMap()
        {
            Console.CursorVisible = false;
            Console.CursorLeft = 53;
            Console.CursorTop = 1;
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
                        // MOUNTAIN
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        map_raw = '▲';
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
                        // SHRUB
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        map_raw = '♣';
                    }
                    else if (map_raw == '4')
                    {
                        // TREE
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Green;
                        map_raw = '↑';
                    }
                    else
                    {
                        System.ArgumentException bad_block = new System.ArgumentException(String.Format("Block with id {0} does not exist", map_raw));
                        throw bad_block;
                    }
                    Console.Write(map_raw);

                    if (Console.CursorLeft == 66 & Console.CursorTop == 8)
                    {
                        // Draw the player
                        Console.CursorLeft--;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('☺');
                    }
                }
                if (Console.CursorTop < 24)
                {
                    Console.ResetColor();
                    Console.Write("|");
                    Console.CursorTop++;
                }
                Console.CursorLeft = 53;
            }
            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public static void Log(string text)
        {
            Regex.Replace(text, "(.{" + 51 + "})", "$1" + Environment.NewLine);
            log_data[log_coords[0]] = ' ' + text;
            log_coords[0] += text.Split('\n').Length;
            Update();
        }

        public static string ReadLine()
        {
            Console.CursorVisible = true;
            Console.Write("» ");
            // Check if the first keypress is an arrow key
            string readline_output = "";
            ConsoleKeyInfo first_keypress = Console.ReadKey();
            if (first_keypress.Key == ConsoleKey.UpArrow)
            {
                Player.Move(1);
            }
            else if (first_keypress.Key == ConsoleKey.RightArrow)
            {
                Player.Move(2);
            }
            else if (first_keypress.Key == ConsoleKey.DownArrow)
            {
                Player.Move(3);
            }
            else if (first_keypress.Key == ConsoleKey.LeftArrow)
            {
                Player.Move(4);
            }
            else
            {
                readline_output = first_keypress.ToString() + Console.ReadLine();
                Console.CursorVisible = false;
                Update();
            }
            return readline_output;
        }

        public static void Clear()
        {
            // Clear log chars
            for (int clear_count = 0; clear_count <= 12; clear_count++)
            {
                Console.Clear();
                frame = new StringBuilder(blank_frame.ToString());
                UpdateMap(Program.PlanetGen.GetPlanet(), Player.pos);
                log_coords[0] = 1;
                log_coords[1] = 1;
            }
        }
    }
}