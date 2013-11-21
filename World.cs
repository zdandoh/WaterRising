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
                if (action_group == 0)
                {
                    // climb
                    if (IsPlayerAdjacent(1))
                    {
                        Player.pos = GetAdjacentBlock(1);
                        UI.Log("You climb the nearest mountain");
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
    }
}
