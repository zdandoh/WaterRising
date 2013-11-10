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
    }
}
