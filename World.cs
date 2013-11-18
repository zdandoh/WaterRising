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
        public static byte[] GetSurround(int[] loc)
        {
            byte[] surroundings = { 0, 0, 0, 0 };
            UI.Log(String.Format("{0} {1}", loc[0], loc[1]));
            byte[,] temp_planet = Program.world;
            surroundings[0] = temp_planet[loc[0] - 1, loc[1]];
            surroundings[1] = temp_planet[loc[0], loc[1] + 1];
            surroundings[2] = temp_planet[loc[0] + 1, loc[1]];
            surroundings[3] = temp_planet[loc[0], loc[1] - 1];
            for (int c = 0; c < surroundings.Length; c++ )
            {
                UI.Log(surroundings[c].ToString());
            }
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
                    int[] res = IsPlayerAdjacent(1);
                    UI.Log(String.Format("{0} {1}, {2} {3}", Player.pos[0], Player.pos[1], res[0], res[1]));
                }
            }
            return "";
        }

        public static int[] IsPlayerAdjacent(int block)
        {
            int[] ppos = Player.pos;
            byte[] surr = GetSurround(ppos);
            int[] return_list = {-1, -1};
            for (int count = 0; count < surr.Length; count++ )
            {
                if (surr[count] == block)
                {
                    // THIS IS AWFUL STYLE I HATE THIS WHOLE FUNCTION
                    if (count == 1)
                    {
                        ppos[0] += 1;
                    }
                    else if (count == 2)
                    {
                        ppos[1] += 1;
                    }
                    else if (count == 3)
                    {
                        ppos[0] -= 1;
                    }
                    else if (count == 4)
                    {
                        ppos[1] -= 1;
                    }
                }
            }
            return ppos;
        }
    }
}
