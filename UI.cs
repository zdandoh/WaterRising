using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class UI
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
|                                                   |-------------------------|
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|                                                   |                         |
|---------------------------------------------------|-------------------------|
");
        public static void Update()
        {
            Console.Clear();
            Console.Write(frame.ToString());
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
                frame[((log_coords[0] * 80) + log_coords[1] + 4)] = c;
                log_coords[1]++;
            }
            log_coords[0]++;
            log_coords[1] = log_coords[0];
        }
    }
}