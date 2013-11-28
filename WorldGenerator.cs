using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    public class WorldGenerator
    {
        static Random RandGen = new Random();
        static Stopwatch watch = new Stopwatch();
        static int size = 1000;
        public static byte[,] planet = new byte[size, size];

        static void RegisterBlock(byte block_id, string name, bool is_solid, char face, ConsoleColor bg_color, ConsoleColor fg_color, string walk_message = "undefined")
        {
            Block new_block = new Block();
            new_block.id = block_id;
            new_block.name = name;
            new_block.is_solid = is_solid;
            new_block.face = face;
            new_block.bg_color = bg_color;
            new_block.fg_color = fg_color;
            new_block.walk_message = walk_message;
            World.blocks.Add(new_block);
        }

        static void RegisterBlocks()
        {
            RegisterBlock(1, "mountain", true, '▲', ConsoleColor.DarkGreen, ConsoleColor.DarkYellow, "Atop a tall mountain");
            RegisterBlock(2, "water", true, '~', ConsoleColor.DarkCyan, ConsoleColor.Cyan, "Freezing cold water");
            RegisterBlock(3, "berry", false, '♣', ConsoleColor.DarkGreen, ConsoleColor.Magenta, "A berry bush, heavy with nature's bounty");
            World.GetBlock(3).feed = 100;
            RegisterBlock(4, "tree", false, '↑', ConsoleColor.DarkGreen, ConsoleColor.Green, "A tall evergreen towers before you");
            RegisterBlock(5, "farm", false, '░', ConsoleColor.DarkGreen, ConsoleColor.Magenta, "A small patch of farmland, created with love and malnutrition");
        }

        public byte[,] MakePlanet()
        {
            RegisterBlocks();
            UI.Log("Placing dirt...");
            UI.Log("Landscaping...");
            planet = RandScatter(planet, 1, 10);
            planet = RandScatter(planet, 4, 7);
            UI.Log("Adding shrubbery...");
            planet = RandScatter(planet, 3, 1000);
            UI.Log("Growing forest...");
            planet = AddBlob(planet, 4, 2000, 10);
            UI.Log("Making Ponds...");
            planet = AddBlob(planet, 2, 100, 1000);
            UI.Log("Raising mountains...");
            planet = AddBlob(planet, 1, 100, 1000);
            UI.Log("Smoothening...");
            // Change all with 4 around to those who surround it
            return planet;
        }

        public byte GetSides(byte[,] planet, int row, int col)
        {
            byte side_count = 0;
            if (planet[row + 1, col] > 0)
            {
                side_count++;
            }
            if (planet[row, col + 1] > 0)
            {
                side_count++;
            }
            if (planet[row - 1, col] > 0)
            {
                side_count++;
            }
            if (planet[row, col - 1] > 0)
            {
                side_count++;
            }
            return side_count;
        }

        public void ShowPlanet(byte[,] array)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(0); col++)
                {
                    Console.Write(array[row, col]);
                }
                Console.WriteLine();
            }
        }

        public byte[,] MixArray(byte[,] source, byte[,] paper)
        {
            // Copy the nonzero elements of one array to another
            for (int row = 0; row < source.GetLength(0); row++)
            {
                for (int col = 0; col < source.GetLength(1); col++)
                {
                    byte copy_byte = paper[row, col];
                    if (copy_byte != 0)
                    {
                        source[row, col] = paper[row, col];
                    }
                }
            }
            return source;
        }

        public byte[,] RandScatter(byte[,] planet, byte tile, int chance)
        {
            byte[,] mask = new byte[1000, 1000];
            for (int row = 0; row < mask.GetLength(0); row++)
            {
                for (int col = 0; col < mask.GetLength(1); col++)
                {
                    int will_replace = RandGen.Next(0, chance);
                    if (will_replace == 1)
                    {
                        planet[row, col] = tile;
                    }
                }
            }
            return planet;
        }

        public byte[,] AddBlob(byte[,] planet, byte block, int size, int seeds = 1)
        {
            // Create mask array and plant the seed
            size = size * seeds;
            byte[,] mask = new byte[1000, 1000];
            for (int seeds_placed = 0; seeds_placed < seeds; seeds_placed++)
            {
                int[] rand_seed = { RandGen.Next(0, 1000), RandGen.Next(0, 1000) };
                mask[rand_seed[0], rand_seed[1]] = block;
            }
            int blob_size = 0;
            watch.Start();
            while (blob_size < size)
            {
                for (int row_count = 0; mask.GetLength(0) > row_count; row_count++)
                {
                    for (int col_count = 0; mask.GetLength(1) > col_count; col_count++)
                    {
                        if (row_count < 999 & col_count < 999 & row_count > 0 & col_count > 0)
                        {
                            byte sides = GetSides(mask, row_count, col_count);
                            if (sides > 0 & mask[row_count, col_count] != block & blob_size <= size)
                            {
                                byte rand_selector = (byte)RandGen.Next(1, 5);
                                if (sides == rand_selector)
                                {
                                    mask[row_count, col_count] = block;
                                    blob_size++;
                                }
                            }
                        }
                    }
                }
            }
            watch.Stop();
            planet = MixArray(planet, mask);
            return planet;
        }
    }

    public class Block
    {
        public byte id;
        public string name;
        public bool is_solid;
        public ConsoleColor bg_color;
        public ConsoleColor fg_color;
        public char face;
        public int feed = 0;
        public string walk_message = "undefined";
    }

    public class Item
    {
        public string name;
        public int id;
        public int qty;
    }
}
