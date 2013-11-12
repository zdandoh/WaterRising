using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterRising
{
    class World
    {
        // Utilities for interacting with the world array
        public static byte[] GetSurround(int[] loc)
        {
            byte[] surroundings = { 0, 0, 0, 0 };
            byte[,] temp_planet = Program.PlanetGen.GetPlanet();
            surroundings[0] = temp_planet[loc[0] + 1, loc[1]];
            surroundings[1] = temp_planet[loc[0], loc[1] + 1];
            surroundings[2] = temp_planet[loc[0] - 1, loc[1]];
            surroundings[3] = temp_planet[loc[0], loc[1] - 1];
            return surroundings;
        }
    }
}
