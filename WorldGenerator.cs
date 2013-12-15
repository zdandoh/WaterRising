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
        public static Random RandGen = new Random();
        static Stopwatch watch = new Stopwatch();
        static int size = 1000;
        public static byte[,] planet = new byte[size, size];

        static void RegisterBlock(byte block_id, string name, bool is_solid, string face, ConsoleColor bg_color, ConsoleColor fg_color, string walk_message = "undefined")
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

        static void RegisterRecipe(string product, byte station, params string[] ingredients)
        {
            Recipe new_recipe = new Recipe();
            new_recipe.product = product;
            new_recipe.station = station;
            foreach (string ingredient in ingredients)
            {
                new_recipe.ingredients.Add(ingredient);
            }
            World.recipes.Add(new_recipe);
        }

        static void RegisterBlocks()
        {
            RegisterBlock(1, "mountain", true, "▲", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow, "Atop a tall mountain");
            RegisterBlock(2, "water", true, "~", ConsoleColor.DarkCyan, ConsoleColor.Cyan, "Freezing cold water");
            RegisterBlock(3, "berry", false, "♣", ConsoleColor.DarkGreen, ConsoleColor.Magenta, "A berry bush, heavy with nature's bounty");
            World.GetBlock(3).feed = 100;
            RegisterBlock(4, "tree", false, "↑", ConsoleColor.DarkGreen, ConsoleColor.Green, "A tall evergreen towers before you");
            RegisterBlock(5, "farm", false, "░", ConsoleColor.DarkGreen, ConsoleColor.Magenta, "A small patch of farmland, created with love and malnutrition");
            RegisterBlock(6, "stone", true, "▲", ConsoleColor.DarkGreen, ConsoleColor.DarkGray, "A large outcropping of stone");
            RegisterBlock(7, "floodwater", true, "~", ConsoleColor.DarkBlue, ConsoleColor.Blue, "Floodwaters, cold as ice");
            RegisterBlock(8, "table", true, "π", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow, "A roughly carved table, perfect for woodwork");
            RegisterBlock(9, "furnace", true, "⌂", ConsoleColor.DarkGreen, ConsoleColor.DarkGray, "A small stone furnace, good for roasting food or smelting metals");
            RegisterBlock(10, "reed", true, "║", ConsoleColor.DarkCyan, ConsoleColor.Yellow, "A sparse cluster of tall reeds");
        }

        static void RegisterRecipes()
        {
            RegisterRecipe("axe", 0, "branch", "stone");
            RegisterRecipe("pick", 0, "branch", "stone");
            RegisterRecipe("table", 0, "log", "log", "log", "log");
            RegisterRecipe("furnace", 0, "stone", "stone", "stone", "stone", "stone");
            RegisterRecipe("plank", 8, "log");
            RegisterRecipe("iron", 9, "ore");
            RegisterRecipe("rope", 8, "reed", "reed", "reed");
            RegisterRecipe("rod", 8, "branch", "iron", "rope");
            RegisterRecipe("fillet", 9, "fish");
        }

        public byte[,] MakePlanet()
        {
            RegisterBlocks();
            RegisterRecipes();
            UI.Log("Placing dirt...");
            UI.Log("Landscaping...");
            planet = RandScatter(planet, 1, 10);
            planet = RandScatter(planet, 4, 7);
            planet = RandScatter(planet, 6, 50);
            UI.Log("Adding shrubbery...");
            planet = RandScatter(planet, 3, 1000);
            UI.Log("Growing forest...");
            planet = AddBlob(planet, 4, 2000, 10);
            UI.Log("Making Ponds...");
            planet = AddBlob(planet, 2, 100, 1000);
            planet = RandScatter(planet, 10, 20);
            UI.Log("Raising mountains...");
            planet = AddBlob(planet, 1, 100, 1000);
            UI.Log("Smoothening...");
            // Change all with 4 around to those who surround it
            for (int row = 1; row < 999; row++)
            {
                for (int col = 1; col < 999; col++)
                {
                    byte[] sides = GetSideIDs(planet, row, col);
                    bool all_same = true;
                    foreach (byte side in sides)
                    {
                        if (side != 2)
                        {
                            all_same = false;
                        }
                    }
                    if (all_same)
                    {
                        planet[row, col] = 2;
                    }
                }
            }
                UI.Log("Starting flood...");
            for (int row = 0; row < planet.GetLength(0); row++)
            {
                for (int col = 0; col < planet.GetLength(1); col++)
                {
                    if (row == 1 || col == 1 || col == 998 || row == 998)
                    {
                        planet[row, col] = 7; // Set to floodwater
                    }
                }
            }
            return planet;
        }

        public byte GetSides(byte[,] planet, int row, int col, int search = 0)
        {
            byte side_count = 0;
            if (planet[row + 1, col] > search)
            {
                side_count++;
            }
            if (planet[row, col + 1] > search)
            {
                side_count++;
            }
            if (planet[row - 1, col] > search)
            {
                side_count++;
            }
            if (planet[row, col - 1] > search)
            {
                side_count++;
            }
            return side_count;
        }

        public byte[] GetSideIDs(byte[,] planet, int row, int col)
        {
            byte[] sides = new byte[4];
            sides[0] = planet[row + 1, col];
            sides[1] = planet[row, col + 1];
            sides[2] = planet[row - 1, col];
            sides[3] = planet[row, col - 1];
            return sides;
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
                    if (will_replace == 0)
                    {
                        if (tile == 10)
                        {
                            if (row != 0 && col != 0 && row != 999 && col != 999)
                            {
                                byte[] tile_sides = GetSideIDs(planet, row, col);
                                bool water_found = false;
                                bool dirt_found = false;
                                bool reed_found = false;
                                foreach (byte side in tile_sides)
                                {
                                    if (side == 0)
                                    {
                                        dirt_found = true;
                                    }
                                    else if (side == 2)
                                    {
                                        water_found = true;
                                    }
                                    else if (side == tile)
                                    {
                                        reed_found = true;
                                    }
                                }
                                if (water_found && dirt_found && reed_found == false)
                                {
                                    //Generate a reed
                                    planet[row, col] = tile;
                                }
                            }
                        }
                        else
                        {
                            planet[row, col] = tile;
                        }
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
        public string face;
        public int feed = 0;
        public string walk_message = "undefined";
    }

    public class Item
    {
        public string name;
        public int id;
        public int qty;
    }

    public class Recipe
    {
        public string product;
        public byte station;
        public List<string> ingredients = new List<string>();

        public bool Craft()
        {
            List<Item> temp_inv = new List<Item>();
            List<string> failed_items = new List<string>();
            bool success = true;
            int item_count = Player.inventory.Count();
            // Take backup of player invent
            foreach (Item item in Player.inventory)
            {
                temp_inv.Add(item);
            }
            if (station > 0 && World.IsPlayerAdjacent(station) == false)
            {
                UI.Log(String.Format("You cannot craft {0}, you are not near a {1}", product, World.GetBlock(station).name));
                success = false;
            }
            foreach (string ingredient in ingredients)
            {
                if (Player.RemoveItem(ingredient) == false)
                {
                    success = false;
                    failed_items.Add(ingredient);
                }
            }
            if (success)
            {
                UI.Log(String.Format("Crafted {0}", product));
                Player.AddItem(product);
            }
            else if (success == false)
            {
                // Reset inventory
                Player.inventory = temp_inv;
                string log_string = String.Format("Cannot craft {0}, missing:", product);
                foreach (string item in failed_items)
                {
                    log_string += (" " + item);
                }
                UI.Log(log_string);
            }
            if (success == false && Player.inventory.Count() != item_count)
            {
                UI.Log("CRAFTING ERROR");
                Console.ReadLine();
            }
            return success;
        }
    }
}
