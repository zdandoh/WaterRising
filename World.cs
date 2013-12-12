﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    // Utilities for interacting with the world array
    class World
    {
        public static List<Block> blocks = new List<Block>();
        public static List<Recipe> recipes = new List<Recipe>();
        public static List<int[]> farms = new List<int[]>();
        public static Random Rand = new Random();
        public static byte[] GetSurround(int[] loc)
        {
            byte[] surroundings = { 0, 0, 0, 0 };
            byte[,] temp_planet = Program.world;
            surroundings[0] = temp_planet[loc[0] - 1, loc[1]];
            surroundings[1] = temp_planet[loc[0], loc[1] + 1];
            surroundings[2] = temp_planet[loc[0] + 1, loc[1]];
            surroundings[3] = temp_planet[loc[0], loc[1] - 1];
            return surroundings;
        }

        public static string Interact(int block, int action_group, int item_group)
        {
            if (action_group == Player.LookupWord("swim"))
            {
                if (IsPlayerAdjacent(2))
                {
                    UI.Log("The water is frigid, swimming is impossible");
                }
                else
                {
                    UI.Log("There is nothing to swim in");
                }
            }
            else if (action_group == Player.LookupWord("craft"))
            {
                // CRAFTING
                bool tried_to_craft = false;
                foreach (Recipe recipe in World.recipes)
                {
                    if (item_group == Player.LookupWord(recipe.product, "item"))
                    {
                        recipe.Craft();
                        tried_to_craft = true;
                    }
                }
                if (tried_to_craft == false)
                {
                    UI.Log("It is impossible to make that");
                }
            }
            else if (action_group == Player.LookupWord("smelt"))
            {
                if (item_group == Player.LookupWord("ore", "item"))
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("iron", "item"));
                }
            }
            else if (block == 1)
            {
                // MOUNTAIN
                if (IsPlayerAdjacent(1))
                {
                    if (action_group == Player.LookupWord("climb"))
                    {
                        Player.pos = GetAdjacentBlock(1);
                        Player.hunger -= 10;
                        UI.Log("You climb the nearest mountain");
                    }
                    else if (action_group == Player.LookupWord("mine"))
                    {
                        UI.Log("The mountain is mostly made of dirt, mining is pointless");
                    }
                }
            }
            else if (block == 2)
            {
                // WATER
                if (action_group == Player.LookupWord("swim"))
                {
                    UI.Log("The water is too cold");
                }
            }
            else if (block == 3)
            {
                // BERRY BUSH
                if (IsPlayerAdjacent(3))
                {
                    if (action_group == Player.LookupWord("eat"))
                    {
                        Player.hunger += GetBlock(3).feed;
                        RemoveAdjacent(3);
                        UI.Log("You pick the bush clean");
                    }
                    else if (action_group == Player.LookupWord("pick"))
                    {
                        // Add berries to player inventory
                        Player.AddItem("berry", Rand.Next(3, 5));
                        RemoveAdjacent(3);
                        UI.Log("You pick the bush clean");
                    }
                }
                else
                {
                    UI.Log("You see no berries nearby");
                }
            }
            else if (block == 4)
            {
                // TREE
                if (IsPlayerAdjacent(4))
                {
                    if (action_group == Player.LookupWord("gather"))
                    {
                        // pick up branches/chop tree
                        Player.AddItem("branch", 1);
                        Player.hunger -= 20;
                        UI.Log("You gather a few branches from a nearby tree");
                    }
                    else if (action_group == Player.LookupWord("chop"))
                    {
                        if (Player.HasItem("axe") >= 0)
                        {
                            Player.AddItem("log", 1);
                            Player.hunger -= 20;
                            UI.Log("You take your axe to the nearest tree");
                            RemoveAdjacent(4);
                        }
                        else
                        {
                            UI.Log("You cannot fell the tree without an axe");
                        }
                    }
                }
            }
            else if (block == 5)
            {
                // Farm
                if (action_group == Player.LookupWord("plant"))
                {
                    if (Player.RemoveItem("berry", 2))
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 5;
                        farms.Add(new int[] {Player.pos[0], Player.pos[1]});
                        UI.Log("You plant a small berry farm");
                    }
                    else
                    {
                        UI.Log("Not enough berries to create a farm!");
                    }
                }
            }
            else if (block == 6)
            {
                // Rock
                if (IsPlayerAdjacent(6))
                {
                    if (action_group == Player.LookupWord("gather"))
                    {
                        UI.Log("You pry free a lose piece of rock");
                        Player.AddItem("rock");
                    }
                    else if (action_group == Player.LookupWord("climb"))
                    {
                        Player.pos = GetAdjacentBlock(6);
                        Player.hunger -= 10;
                        UI.Log("You climb atop the large boulder");
                    }
                    else if (action_group == Player.LookupWord("mine"))
                    {
                        if (Player.HasItem("pick") > -1)
                        {
                            UI.Log("You take to the boulder with your pickaxe");
                            Player.hunger -= 20;
                            Player.AddItem("stone", Rand.Next(1, 3));
                            if (Rand.Next(0, 4) == 2)
                            {
                                // Give ferrous ore
                                Player.AddItem("ore");
                                UI.Log("You find a ferrous ore deposit in the boulder");
                            }
                            RemoveAdjacent(6);
                        }
                        else
                        {
                            UI.Log("You cannot mine without a pickaxe!");
                        }
                    }
                }
            }
            else if (block == 8)
            {
                if (action_group == Player.LookupWord("place"))
                {
                    if (Player.HasItem("table") > -1)
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 8;
                        UI.Log("Table placed");
                    }
                }
            }
            else if (block == 9)
            {
                if (action_group == Player.LookupWord("place"))
                {
                    if (Player.HasItem("furnace") > -1)
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 9;
                        UI.Log("Furnace placed");
                    }
                }
            }
            UI.UpdateMap(Program.world, Player.pos);
            return "";
        }

        public static bool IsPlayerAdjacent(int block)
        {
            bool next_to = false;
            byte[] surr = GetSurround(Player.pos);
            foreach (byte adj_block in surr)
            {
                if (adj_block == block)
                {
                    next_to = true;
                }
            }
            return next_to;
        }

        public static void Update()
        {
            for (int farm_count = 0; farm_count < farms.Count; farm_count++)
            {
                // Iterate through every farm
                if (Rand.Next(0, 70) == 2)
                {
                    // Farm grows
                    int[] farm_location = farms[farm_count];
                    Program.world[farm_location[0], farm_location[1]] = 3;
                    farms.RemoveAt(farm_count);
                }
            }
            // Progress flood
            for (int row = 0; row < 999; row++)
            {
                for (int col = 0; col < 999; col++)
                {
                    if (Program.world[row, col] == 7)
                    {
                        if (Rand.Next(2) == 1)
                        {
                            if (col == 999 || col == 0 || row == 999 || row == 0)
                            {
                                { };
                            }
                            else
                            {
                                Program.world[row + 1, col] = 7;
                                Program.world[row - 1, col] = 7;
                                Program.world[row, col + 1] = 7;
                                Program.world[row, col - 1] = 7;
                            }
                        }
                    }
                }
            }
        }

        public static void RemoveAdjacent(int block)
        {
            int[] block_coords = GetAdjacentBlock(block);
            Program.world[block_coords[0], block_coords[1]] = 0;
        }

        public static bool Craft(string name)
        {
            bool success = false;
            if (Player.LookupWord(name) == Player.LookupWord("axe"))
            {
                
            }
            return success;
        }

        public static int[] GetAdjacentBlock(int block)
        {
            int[] ppos = Player.pos;
            byte[] surr = GetSurround(ppos);
            int[] return_list = {-1, -1};
            for (int count = 1; count < surr.Length + 1; count++ )
            {
                if (surr[count - 1] == block)
                {
                    // THIS IS AWFUL I HATE THIS WHOLE FUNCTION
                    if (count == 1)
                    {
                        ppos[0] -= 1;
                        break;
                    }
                    else if (count == 2)
                    {
                        ppos[1] += 1;
                        break;
                    }
                    else if (count == 3)
                    {
                        ppos[0] += 1;
                        break;
                    }
                    else if (count == 4)
                    {
                        ppos[1] -= 1;
                        break;
                    }
                }
            }
            return ppos;
        }
        public static Block GetBlock(byte id)
        {
            foreach (Block block in World.blocks)
            {
                if (block.id == id)
                {
                    return block;
                }
            }
            return null;
        }

        public static Block GetBlock(string name)
        {
            foreach (Block block in World.blocks)
            {
                if (block.name == name)
                {
                    return block;
                }
            }
            return null;
        }

        public static Recipe GetRecipe(string name)
        {
            foreach (Recipe recipe in recipes)
            {
                if (recipe.product == name)
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}
