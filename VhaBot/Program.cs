using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace VhaBot.Core
{
    public class Program
    {
        private static string _config = "config.xml";
        public static void Main(string[] args)
        {
            // Parse arguments
            if (args.Length > 0)
            {
                Program._config = string.Join(" ", args);
            }

            // Display header
            Console.Title = "VhaBot.Core " + Core.VERSION + " " + Core.BRANCH + " (" + Core.BUILD + ") [Config: " + Program._config + "]";
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            WriteLine(@" _    _ _      ___________        _    ", true);
            WriteLine(@"| |  | | |    (_________  \      | |   ", true);
            WriteLine(@"| |  | | |__   ____ ____)  ) ___ | |_  ", true);
            WriteLine(@"| |  | |  _ \ / _  |  __  ( / _ \|  _) ", true);
            WriteLine(@" \ \/ /| | | ( (_| | |__)  ) |_| | |__ ", true);
            WriteLine(@"  \__/ |_| |_|\____|______/ \___/ \___)", true);
            Console.WriteLine();
            WriteMessage("Multi-Bot System", ConsoleColor.Green);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Thread.Sleep(1000);
            try { Console.Clear(); }
            catch { }

            // Start Core
            Core core = new Core(Program._config);
            core.Loop();
        }

        private static void WriteMessage(string text, ConsoleColor color)
        {
            int size = 75;
            try { size = Console.WindowWidth - 5; }
            catch { }
            if (text.Length <= size)
            {
                int spaces = size - text.Length;
                string spaceleft = "";
                string spaceright = "";
                for (int i = 0; i < spaces; i++)
                {
                    int remainder;
                    Math.DivRem(i, 2, out remainder);
                    if (remainder == 1) spaceleft += " ";
                    else spaceright += " ";
                }
                lock (Console.Out)
                {
                    Console.Write(spaceleft + "[ ");
                    try { Console.ForegroundColor = color; }
                    catch { }
                    Console.Write(text);
                    try { Console.ResetColor(); }
                    catch { }
                    Console.WriteLine(" ]" + spaceright);
                }
            }
            else
            {
                WriteMessage(text.Substring(0, size), color);
                WriteMessage(text.Substring(size), color);
            }
        }

        private static void WriteLine(string text) { WriteLine(text, false); }
        private static void WriteLine(string text, bool center)
        {
            int size = 79;
            try { size = Console.WindowWidth - 1; }
            catch { }
            if (text.Length <= size)
            {
                int spaces = size - text.Length;
                string spaceleft = "";
                string spaceright = "";
                for (int i = 0; i < spaces; i++)
                {
                    if (center)
                    {
                        int remainder;
                        Math.DivRem(i, 2, out remainder);
                        if (remainder == 1) spaceleft += " ";
                        else spaceright += " ";
                    }
                    else spaceright += " ";
                }
                lock (Console.Out)
                    Console.WriteLine(spaceleft + text + spaceright);
            }
            else
            {
                WriteLine(text.Substring(0, size));
                WriteLine(text.Substring(size));
            }
        }
    }
}
