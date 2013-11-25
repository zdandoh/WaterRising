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

        public static string Interact(int block, int action_group)
        {
            string response = "";
            if (block == 1)
            {
                // MOUNTAIN
                if (action_group == Player.LookupWord("climb"))
                {
                    if (IsPlayerAdjacent(1))
                    {
                        Player.pos = GetAdjacentBlock(1);
                        Player.hunger -= 10;
                        UI.Log("You climb the nearest mountain");
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
                        // pick up branches
                        Player.AddItem("branch", 1);
                        Player.hunger -= 20;
                        UI.Log("You gather a few branches from a nearby tree");
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

        public static void RemoveAdjacent(int block)
        {
            int[] block_coords = GetAdjacentBlock(block);
            Program.world[block_coords[0], block_coords[1]] = 0;
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
                    // THIS IS AWFUL STYLE I HATE THIS WHOLE FUNCTION
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
    }
}
