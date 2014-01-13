using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    class Player
    {
        public static int[] pos = { 500, 500 };
        public static int health = 1000;
        public static int hunger = 1000;
        public static int move_cost = 2;
        public static bool mountain_move = false;
        public static bool can_move = true;
        public static List<Item> inventory = new List<Item>();
        public static Stopwatch MoveTimer = new Stopwatch();
        public static string last_command = "";
        public static List<string[]> verbs = LoadWords("verbs");
        public static List<string[]> blocks = LoadWords("blocks");
        public static List<string[]> items = LoadWords("items");

        public static void Move(int dir)
        {
            if (MoveTimer.ElapsedMilliseconds > 225 && can_move)
            {
                MoveTimer.Restart();
                int[] old_pos = (int[])pos.Clone();
                if (dir == 1)
                {
                    pos[0]--;
                }
                else if (dir == 2)
                {
                    pos[1]++;
                }
                else if (dir == 3)
                {
                    pos[0]++;
                }
                else if (dir == 4)
                {
                    pos[1]--;
                }
                else
                {
                    // The lazy man's exception
                    Console.WriteLine("Invalid move dir of {0}", dir);
                    Console.ReadLine();
                }
                // Check if we're trying to move into a solid block
                byte new_player_blockid = Program.world[pos[0], pos[1]];
                if (new_player_blockid != 1)
                {
                    mountain_move = false;
                    Player.move_cost = 2;
                }
                foreach (Block block in World.blocks)
                {
                    if (new_player_blockid == block.id)
                    {
                        if (block.is_solid == true)
                        {
                            if (block.id == 1 && mountain_move)
                            {
                                // Can move through mountains
                            }
                            else
                            {
                                pos = old_pos;
                            }
                        }
                    }
                }
                Player.RemoveHunger(move_cost);
                UI.UpdateMap(Program.world, pos);
            }
        }

        public static void AddItem(string name, int amount = 1)
        {
            int id = LookupWord(name, "item");
            int already_have = HasItem(name);
            if (already_have != -1)
            {
                inventory[already_have].qty += amount;
            }
            else
            {
                Item to_add = new Item();
                to_add.name = name;
                to_add.id = LookupWord(name, "item");
                to_add.qty = amount;
                inventory.Add(to_add);
            }
        }

        public static bool RemoveItem(string name, int count = 1)
        {
            bool success = false;
            int item_count = CountItem(name);
            if (item_count == count)
            {
                inventory.RemoveAt(HasItem(name));
                success = true;
            }
            else if (item_count > count)
            {
                inventory[HasItem(name)].qty -= count;
                success = true;
            }
            return success;
        }

        public static int HasItem(string name, int count = 1)
        {
            int id = LookupWord(name, "item");
            int return_index = -1;
            for (int item_no = 0; item_no < inventory.Count; item_no++)
            {
                if (inventory[item_no].id == id)
                {
                    return_index = item_no;
                }
            }
            return return_index;
        }

        public static int CountItem(string name)
        {
            int count = 0;
            int index = HasItem(name);
            if (index > -1)
            {
                count = inventory[index].qty;
            }
            return count;
        }

        public static void RemoveHunger(int cost)
        {
            if (hunger > 0)
            {
                hunger -= cost;
            }
            else if (health > 0)
            {
                health -= cost;
            }
            else
            {
                can_move = false;
                UI.Log("You died");
            }
        }

        public static void AddHunger(int amount)
        {
            if (hunger > 1000)
            {
                UI.Log("You hunger is already full!");
            }
            else
            {
                hunger += amount;
            }
        }

        public static int GetScore()
        {
            int score = 0;
            // +25 points for each item in invent
            int total_items = 0;
            foreach (Item item in inventory)
            {
                total_items += item.qty;
            }
            score += total_items * 25;
            // Remaining life and hunger
            score += health;
            score += hunger;
            // Count up food
            score += (Player.CountItem("berry") * 50);
            score += (Player.CountItem("fillet") * 60);
            // Treasures
            score += (Player.CountItem("nugget") * 100);
            score += (Player.CountItem("diamond") * 1000);
            score += (Player.CountItem("squid") * 400);
            return score;
        }

        public static List<string[]> LoadWords(string file_name)
        {
            //Split apart verbs & block synonyms
            List<string[]> word_container = new List<string[]>();
            string rec_file = "";
            if (file_name == "verbs")
            {
                rec_file = Properties.Resources.Verbs;
            }
            else if (file_name == "blocks")
            {
                rec_file = Properties.Resources.Blocks;
            }
            else if (file_name == "items")
            {
                rec_file = Properties.Resources.items;
            }
            string[] actions = rec_file.Replace("{", "").Split('}'); // Retrives seperate arrays of all possible actions
            foreach (string synonyms in actions)
            {
                string[] split_syn = synonyms.Split(new string[] { ", " }, StringSplitOptions.None);
                word_container.Add(split_syn);
            }
            return word_container;
        }

        public static int LookupWord(string str, string type = "verb")
        {
            List<string[]> all_cats = new List<string[]>();
            if (type == "verb")
            {
                all_cats = verbs;
            }
            else if (type == "block")
            {
                all_cats = blocks;
            }
            else if (type == "item")
            {
                all_cats = items;
            }
            else
            {
                Console.WriteLine("Incorrect call of LookupWord {0} is not valid type", type);
                Console.ReadLine();
            }
            for (int catagory = 0; catagory < all_cats.Count(); catagory++)
            {
                for (int option = 0; option < all_cats[catagory].GetLength(0); option++)
                {
                    if (str.Equals(all_cats[catagory][option].Trim()))
                    {
                        // It's in the db, return group #
                        return catagory;
                    }
                }
            }
            return -1;
        }

        public static void HandleInput(string command)
        {
            // Check if it's a special command
            if (command == "r")
            {
                command = last_command;
            }
            else if (command.Contains("fastforward"))
            {
                int update_count = Convert.ToInt32(command.Split(' ')[1]);
                UI.Log("Fast forwarding world...");
                for (int updates_done = 0; updates_done < update_count; updates_done++)
                {
                    World.Update();
                }
                UI.Log("Fast forward complete!");
            }
            else if (command == "score")
            {
                UI.Log(String.Format("Current score: {0}", GetScore()));
            }
            else if (command == "scores")
            {
                string scores = Program.GetScores();
                UI.Log("High Scores:");
                foreach (string score_line in scores.Split('\n'))
                {
                    UI.Log(score_line);
                }
            }
            else if (command == "save")
            {
                // Save the game world and state
                UI.Log("Saving...");
                Program.Save();
                UI.Log("Saved!");
            }
            else if (command == "load")
            {
                // Load the latest world
                Program.Load();
                UI.Log("Loaded!");
            }
            else
            {
                last_command = command;
            }
            // Extract verb and object, really frickin' breakable atm
            string[] words = command.Split(' ');
            string verb_word = "";
            string block_word = "";
            string item_word = "";
            int verb_group = -1;
            int block_group = -1;
            int item_group = -1;
            foreach (string word in words)
            {
                int verb_result = LookupWord(word, "verb");
                int block_result = LookupWord(word, "block");
                int item_result = LookupWord(word, "item");
                if (verb_result > -1 & verb_group == -1)
                {
                    verb_group = verb_result;
                    verb_word = word;
                }
                if (block_result > -1)
                {
                    block_group = block_result;
                    block_word = word;
                }
                if (item_result > -1)
                {
                    item_group = item_result;
                    item_word = word;
                }
            }
            World.Interact(block_group, verb_group, item_group, false, verb_word, block_word, item_word);
        }
    }
}
