using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    class Player
    {
        public static int[] pos = { 500, 500 };
        public static byte[] env = ReadEnv();
        public static List<string[]> verbs = LoadWords("verbs");
        public static List<string[]> blocks = LoadWords("blocks");
        static int last_move = 1000;

        public static void Move(int dir)
        {
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
            UI.UpdateMap(Program.PlanetGen.GetPlanet(), pos);
        }

        public static byte[] ReadEnv()
        {
            byte[] surroundings = new byte[5];
            surroundings[0] = Program.PlanetGen.GetPlanet()[pos[0], pos[1]];
            surroundings[1] = Program.PlanetGen.GetPlanet()[pos[0] - 1, pos[1]];
            surroundings[2] = Program.PlanetGen.GetPlanet()[pos[0], pos[1] + 1];
            surroundings[3] = Program.PlanetGen.GetPlanet()[pos[0] + 1, pos[1]];
            surroundings[4] = Program.PlanetGen.GetPlanet()[pos[0], pos[1] - 1];
            return surroundings;
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
            string[] actions = rec_file.Replace("{", "").Split('}'); // Retrives seperate arrays of all possible actions
            foreach (string synonyms in actions)
            {
                string[] split_syn = synonyms.Split(new string[] { ", " }, StringSplitOptions.None);
                word_container.Add(split_syn);
            }
            return word_container;
        }

        public static int LookupWord(string str, string type)
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
            // Extract verb and object, really frickin' breakable atm
            string[] words = command.Split(' ');
            int verb_group = -1;
            int block_group = -1;
            foreach (string word in words)
            {
                int verb_result = LookupWord(word, "verb");
                int block_result = LookupWord(word, "block");
                if (verb_result > -1)
                {
                    verb_group = verb_result;
                }
                if (block_result > -1)
                {
                    block_group = block_result;
                }
            }
            UI.Log(String.Format("{0}, {1}", verb_group, block_group));
        }
    }
}
