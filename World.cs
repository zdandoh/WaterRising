using System;
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

        public static string Interact(int block, int action_group, int item_group, bool followup = false, string verb_word = "", string block_word = "", string item_word = "")
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
                //Create a farm
                if (block == Player.LookupWord("farm", "block"))
                {
                    if (Player.RemoveItem("berry", 2))
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 5;
                        farms.Add(new int[] { Player.pos[0], Player.pos[1] });
                        UI.Log("You plant a small berry farm");
                    }
                    else
                    {
                        UI.Log("Not enough berries to create a farm!");
                    }
                }
                else if (item_group == Player.LookupWord("boat", "item") && followup == false)
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("boat", "item"), true);
                    if (Player.RemoveItem("boat"))
                    {
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 0] = 0;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 1] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 2] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 3] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 4] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 5] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 6] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 7] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 8] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 9] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 10] = 12;
                        Program.world[Player.pos[0] + 0, Player.pos[1] + 11] = 12;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 0] = 12;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 1] = 12;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 2] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 3] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 4] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 5] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 6] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 7] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 8] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 9] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 10] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 11] = 11;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 12] = 12;
                        Program.world[Player.pos[0] + 1, Player.pos[1] + 13] = 12;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 0] = 12;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 1] = 12;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 2] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 3] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 4] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 5] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 6] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 7] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 8] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 9] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 10] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 11] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 12] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 13] = 11;
                        Program.world[Player.pos[0] + 2, Player.pos[1] + 14] = 12;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 0] = 12;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 1] = 12;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 2] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 3] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 4] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 5] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 6] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 7] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 8] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 9] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 10] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 11] = 11;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 12] = 12;
                        Program.world[Player.pos[0] + 3, Player.pos[1] + 13] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 0] = 0;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 1] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 2] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 3] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 4] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 5] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 6] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 7] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 8] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 9] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 10] = 12;
                        Program.world[Player.pos[0] + 4, Player.pos[1] + 11] = 12;
                    }
                }
                else if (block == Player.LookupWord("table", "block") && followup == false)
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("table", "item"), true);
                    if (Player.RemoveItem("table"))
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 8;
                    }
                }
                else if (block == Player.LookupWord("furnace", "block") && followup == false)
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("furnace", "item"), true);
                    if (Player.RemoveItem("furnace"))
                    {
                        Program.world[Player.pos[0], Player.pos[1]] = 9;
                    }
                }
                else
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
                        UI.Log("It is impossible to make a that");
                    }
                }
            }
            else if (action_group == Player.LookupWord("smelt"))
            {
                if (item_group == Player.LookupWord("ore", "item"))
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("iron", "item"));
                }
                else if (item_group == Player.LookupWord("fish", "item"))
                {
                    Interact(-1, Player.LookupWord("craft"), Player.LookupWord("fillet", "item"));
                }
            }
            else if (action_group == Player.LookupWord("fish"))
            {
                if (IsPlayerAdjacent(2))
                {
                    if (Player.HasItem("rod") > -1)
                    {
                        UI.Log("You cast your rod and wait for a bite");
                        Player.RemoveHunger(10);
                        int path = Rand.Next(0, 101);
                        if (path <= 40)
                        {
                            UI.Log("You pull out a large fish!");
                            Player.AddItem("fish");
                        }
                        else if (path == 100)
                        {
                            UI.Log("You haul up a gigantic squid!");
                            Player.AddItem("squid");
                        }
                        else
                        {
                            UI.Log("The fish don't seem to be biting...");
                        }
                    }
                    else
                    {
                        UI.Log("You need a fishing rod to go fishing!");
                    }
                }
                else
                {
                    UI.Log("There is no water to fish in!");
                }
            }
            else if (item_group == Player.LookupWord("fillet", "item"))
            {
                if (action_group == Player.LookupWord("eat") && Player.RemoveItem("fillet"))
                {
                    UI.Log("You scarf down the scalding fillet");
                    Player.hunger += 75;
                }
                else
                {
                    UI.Log("You don't have a fillet to speak of!");
                }
            }
            else if (action_group == Player.LookupWord("pan"))
            {
                if (Player.HasItem("pan") > -1)
                {
                    if (IsPlayerAdjacent(2))
                    {
                        Player.RemoveHunger(15);
                        UI.Log("You begin to sift through the silt in search of treasure");
                        int pan_change = Rand.Next(0, 100);
                        if (pan_change == 99)
                        {
                            UI.Log("You find a small diamond in the rough!");
                            Player.AddItem("diamond");
                        }
                        if (pan_change > 85)
                        {
                            UI.Log("You find a golden nugget!");
                            Player.AddItem("nugget");
                        }
                        else if (pan_change < 40)
                        {
                            UI.Log("You find a clump of ferrous ore in your pan!");
                            Player.AddItem("ore");
                        }
                        else
                        {
                            UI.Log("Your pan contains only dirt...");
                        }
                    }
                    else
                    {
                        UI.Log("You can't go panning unless you are near water!");
                    }
                }
                else
                {
                    UI.Log("You can't go panning without a pan!");
                }
            }
            else if (action_group == Player.LookupWord("mine"))
            {
                if (IsPlayerAdjacent(6))
                {
                    if (Player.HasItem("pick") > -1)
                    {
                        UI.Log("You take to the boulder with your pickaxe");
                        Player.RemoveHunger(20);
                        Player.AddItem("stone", Rand.Next(1, 3));
                        if (Rand.Next(0, 3) == 0)
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
                else if (IsPlayerAdjacent(1))
                {
                    UI.Log("The mountain is mostly made of dirt, mining is pointless");
                }
                else
                {
                    UI.Log("There is no suitable place to mine nearby");
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
                        Player.RemoveHunger(5);
                        UI.Log("You climb the nearest mountain");
                    }
                    else if (action_group == Player.LookupWord("gather"))
                    {
                        UI.Log("There appear to be no useful resources on this mountain");
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
                if (action_group == Player.LookupWord("eat"))
                {
                    if (IsPlayerAdjacent(3))
                    {
                        Player.hunger += GetBlock(3).feed;
                        RemoveAdjacent(3);
                        UI.Log("You pick the bush clean");
                    }
                    else
                    {
                        if (Player.HasItem("berry", 3) > -1)
                        {
                            UI.Log("You scarf down a few berries from your bag");
                            Player.hunger += 100;
                            Player.RemoveItem("berry", 3);
                        }
                        else
                        {
                            UI.Log("You see no berries nearby");
                        }
                    }
                }
                else if (IsPlayerAdjacent(3))
                {
                    if (action_group == Player.LookupWord("pick"))
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
                        Player.RemoveHunger(10);
                        UI.Log("You gather a few branches from a nearby tree");
                    }
                    else if (action_group == Player.LookupWord("chop"))
                    {
                        if (Player.HasItem("axe") >= 0)
                        {
                            Player.AddItem("log", 1);
                            Player.RemoveHunger(15);
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
                        Player.RemoveHunger(5);
                        UI.Log("You climb atop the large boulder");
                    }
                }
            }
            else if (block == 10)
            {
                if (action_group == Player.LookupWord("gather"))
                {
                    UI.Log("You gather a small bundle of reeds");
                    Player.RemoveHunger(5);
                    RemoveAdjacent(10, 2);
                    Player.AddItem("reed");
                }
            }
            else
            {
                //Try to let the player know what they're doing wrong
                if (action_group > -1 && block == -1 && item_group == -1)
                {
                    // Only an action
                    UI.Log(String.Format("{0} what?", verb_word));
                }
                else if (action_group == -1 && block > -1 && item_group == -1)
                {
                    // Only a block
                    UI.Log(String.Format("Do what to {0}?", block_word));
                }
                else if (action_group == -1 && block == -1 && item_group > -1)
                {
                    // Only an action
                    UI.Log(String.Format("Do what with {0}?", item_word));
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
                                //hairy
                                if (Program.world[row + 1, col] != 11 && Program.world[row + 1, col] != 12)
                                {
                                    Program.world[row + 1, col] = 7;
                                }
                                if (Program.world[row - 1, col] != 11 && Program.world[row - 1, col] != 12)
                                {
                                    Program.world[row - 1, col] = 7;
                                }
                                if (Program.world[row, col + 1] != 11 && Program.world[row, col + 1] != 12)
                                {
                                    Program.world[row, col + 1] = 7;
                                }
                                if (Program.world[row, col - 1] != 11 && Program.world[row, col - 1] != 12)
                                {
                                    Program.world[row, col - 1] = 7;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RemoveAdjacent(int block, byte new_block = 0)
        {
            int[] block_coords = GetAdjacentBlock(block);
            Program.world[block_coords[0], block_coords[1]] = new_block;
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
