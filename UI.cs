using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WaterRising
{
    public class UI
    {
        static string[,] map = new string[15, 25];
        static int[] map_start = { 1, 53 };
        static int[] log_coords = { 1, 1 };
        static bool map_exists = false;
        static int last_tick = 0;
        static int last_health = Player.health;
        static int last_hunger = Player.hunger;
        static string last_info = "itsover9000";
        static Dictionary<char, int> key_list = new Dictionary<char,int>{
        {'a', 0x41},
        {'b', 0x42},
        {'c', 0x43},
        {'d', 0x44},
        {'e', 0x45},
        {'f', 0x46},
        {'g', 0x47},
        {'h', 0x48},
        {'i', 0x49},
        {'j', 0x4A},
        {'k', 0x4B},
        {'l', 0x4C},
        {'m', 0x4D},
        {'n', 0x4E},
        {'o', 0x4F},
        {'p', 0x50},
        {'q', 0x51},
        {'r', 0x52},
        {'s', 0x53},
        {'t', 0x54},
        {'u', 0x55},
        {'v', 0x56},
        {'w', 0x57},
        {'x', 0x58},
        {'y', 0x59},
        {'z', 0x5A},
        };
        static StringBuilder frame = new StringBuilder(@"|---------------------------------------------------|-------------------------|
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |
|                                                   |-------------------------|
|                                                   | Health:
|                                                   |                         |
|                                                   | Hunger:
|                                                   |                         |
|---------------------------------------------------| 
|»                                                  | 
|---------------------------------------------------|-------------------------|
");
        static List<string> log_data = new List<string>();
        static StringBuilder blank_frame = new StringBuilder(frame.ToString());
        static StringBuilder blank_log = new StringBuilder(log_data.ToString());
        public static void Update()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write(frame.ToString());

            for (int log_entry = 1; log_entry < log_data.Count; log_entry++)
            {
                Console.CursorTop = log_entry;
                Console.CursorLeft = 1;
                Console.Write(log_data[log_entry]);
            }
            if (map_exists == true)
            {
                DrawMap();
            }
            UpdateStatus();
            UpdateInfo();
            Console.CursorLeft = 1;
            Console.CursorTop = 22;
        }

        public static void UpdateStatus()
        {
        // Update health
        Console.SetCursorPosition(62, 17);
        Console.Write("               ");
        Console.BackgroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(62, 17);
        for (int lentils_drawn = 0; lentils_drawn < (Player.health / 32.5); lentils_drawn++)
            Console.Write(" ");
        Console.ResetColor();
        Console.SetCursorPosition(77, 17);
        Console.Write(" |");
        last_health = Player.health;
        // Update hunger
        Console.SetCursorPosition(62, 19);
        Console.Write("               ");
        Console.BackgroundColor = ConsoleColor.DarkCyan;
        Console.SetCursorPosition(62, 19);
        for (int lentils_drawn = 0; lentils_drawn < (Player.hunger / 65); lentils_drawn++)
            Console.Write(" ");
        Console.ResetColor();
        Console.SetCursorPosition(77, 19);
        Console.Write(" |");
        last_hunger = Player.hunger;
        }

        public static void UpdateInfo()
        {
            if (Program.world_done)
            {
                byte player_block = Program.world[Player.pos[0], Player.pos[1]];
                string info_string = "";
                if (player_block > 0)
                {
                    info_string = World.GetBlock(player_block).walk_message;
                }
                else
                {
                    info_string = "A plain of lush grass";
                }
                if (info_string != last_info)
                {
                    //Update info
                    List<string> split_string = new List<string>(Regex.Split(info_string, @"(?<=\G.{23})", RegexOptions.Singleline));
                    Console.SetCursorPosition(54, 21);
                    Console.Write("                        ");
                    Console.SetCursorPosition(54, 22);
                    Console.Write("                        ");
                    for (int split_index = 0; split_index < split_string.Count(); split_index++ )
                    {
                        Console.SetCursorPosition(54, 21 + split_index);
                        Console.Write(split_string[split_index]);
                    }
                    Console.SetCursorPosition(78, 21);
                    Console.Write("|");
                    Console.SetCursorPosition(78, 22);
                    Console.Write("|");
                    last_info = info_string;
                }
            }
        }

        public static void UpdateMap(byte[,] planet, int[] player)
        {
            map_exists = true;
            bool map_flooded = true;
            int[] top_left = { player[0] - 7, player[1] - 12 };
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 25; col++)
                {
                    // iterate through every map byte
                    byte sliced_tile = planet[top_left[0] + row, top_left[1] + col];
                    if (sliced_tile != 11 && sliced_tile != 12 && sliced_tile != 7)
                    {
                        map_flooded = false;
                    }
                    string map_raw = sliced_tile.ToString();
                    map[row, col] = map_raw;
                    // frame[(map_start[0] + row) * 81 + (map_start[1] + col) + 2] = map_raw;
                }
            }
            if (map_flooded)
            {
                Program.flood_complete = true;
            }
            Update();
        }

        public static void DrawMap()
        {
            Console.CursorLeft = 53;
            Console.CursorTop = 1;
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 25; col++)
                {
                    string map_raw = map[row, col];
                    Console.ResetColor();
                    if (map_raw == "0")
                    {
                        // GROUND
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        map_raw = " ";
                    }
                    else if (map_raw != "0")
                    {
                        Block block = World.GetBlock(Byte.Parse(map_raw.ToString()));
                        Console.BackgroundColor = block.bg_color;
                        Console.ForegroundColor = block.fg_color;
                        map_raw = block.face;
                    }
                    else
                    {
                        System.ArgumentException bad_block = new System.ArgumentException(String.Format("Block with id {0} does not exist", map_raw));
                        throw bad_block;
                    }
                    Console.Write(map_raw);

                    if (Console.CursorLeft == 66 & Console.CursorTop == 8)
                    {
                        // Draw the player
                        Console.CursorLeft--;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('☺');
                    }
                }
                if (Console.CursorTop < 24)
                {
                    Console.ResetColor();
                    Console.Write("|");
                    Console.CursorTop++;
                }
                Console.CursorLeft = 53;
            }
            Console.ResetColor();
        }

        public static void Log(string text)
        {
            // Split the input string into 49 character substrings
            char[] text_chars = text.ToCharArray();
            List<string> text_lines = new List<string>();
            string line = "";
            foreach (char text_char in text_chars)
            {
                if (line.Length >= 49)
                {
                    text_lines.Add(line);
                    line = "";
                }
                line += text_char;
            }
            if (line.Length >= 1)
            {
                text_lines.Add(line);
            }
            // Iterate through all substrings and add them to the log data
            foreach (string full_line in text_lines)
            {
                log_data.Add(" " + full_line);
                if (log_data.Count > 21)
                {
                    log_data.RemoveAt(0);
                }
            }
            Update();
        }

        public enum KeyCode : int
        {
            Left = 0x25,
            Up,
            Right,
            Down
        }

        public static class NativeKeyboard
        {
            private const int KeyPressed = 0x8000;

            public static bool IsKeyDown(KeyCode key)
            {
                return (GetKeyState((int)key) & KeyPressed) != 0;
            }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern short GetKeyState(int key);
        }

        public static string ReadLine(char first_key)
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            Program.TickTimer.Stop();
            // Block the process until they backspace out or hit enter
            string readline_output = first_key + "";
            ConsoleKey last_key = (ConsoleKey)'n';
            while (readline_output.Length > 0 && last_key != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(3, 22);
                Console.Write("                                                 ");
                Console.SetCursorPosition(3, 22);
                Console.Write(readline_output);
                last_key = Console.ReadKey(true).Key;
                if (last_key == ConsoleKey.Backspace || last_key == ConsoleKey.Delete)
                {
                    readline_output = readline_output.Substring(0, readline_output.Length - 1);
                }
                else
                {
                    readline_output += char.ToLower((char)last_key);
                }
            }
            Program.TickTimer.Start();
            Update();
            return readline_output;
        }

        public static string ReadInput()
        {
            Console.SetCursorPosition(2, 22);
            // Check if the first keypress is an arrow key
            string readline_output = "";
            if (NativeKeyboard.GetKeyState(0x26) < 0)
            {
                Player.Move(1);
            }
            else if (NativeKeyboard.GetKeyState(0x27) < 0)
            {
                Player.Move(2);
            }
            else if (NativeKeyboard.GetKeyState(0x28) < 0)
            {
                Player.Move(3);
            }
            else if (NativeKeyboard.GetKeyState(0x25) < 0)
            {
                Player.Move(4);
            }
            else if (NativeKeyboard.GetKeyState(0x31) < 0)
            {
                // Show inventory
                UI.Log("Inventory Contents:");
                foreach (Item item in Player.inventory)
                {
                    UI.Log(String.Format("{0}, qty {1}", item.name, item.qty));
                }
            }
            else if (NativeKeyboard.GetKeyState(0x32) < 0)
            {
                // Repeat action
                Player.HandleInput(Player.last_command);
            }
            else if (NativeKeyboard.GetKeyState(0x33) < 0)
            {
                // Repeat action
                Player.HandleInput(Player.second_last_command);
            }
            // The following line is too damn long
            foreach (KeyValuePair<char, int> pair in key_list)
            {
                if (NativeKeyboard.GetKeyState(pair.Value) < 0)
                {
                    readline_output = ReadLine(pair.Key);
                }
            }
            return readline_output.ToLower().TrimEnd('\r', '\n');
        }
    }
}