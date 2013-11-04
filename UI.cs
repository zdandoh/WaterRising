﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    public class UI
    {
        static byte[,] map = new byte[15,25];
        static int[] map_start = { 1, 53 };
        static int[] log_coords = { 1, 1 };
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
            Console.CursorLeft = 1;
            Console.CursorTop = 22;
        }

        public static void UpdateMap(byte[,] planet, int[] player)
        {
            int[] top_left = { player[0] - 8, player[1] - 13};
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 25; col++)
                {
                    // iterate through every map byte
                    char map_raw = planet[top_left[0] + row, top_left[1] + col].ToString().Single();
                    frame[(map_start[0] + row) * 81 + (map_start[1] + col) + 2] = map_raw;
                }
            }
            Update();
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
                Console.WriteLine(log_coords[0]);
                Console.WriteLine(log_coords[1]);
            }
        }
    }
}