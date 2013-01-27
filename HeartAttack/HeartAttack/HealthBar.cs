using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public class HealthBar : Drawable
    {
        private bool _changed = false;
        private int level;

        public int Health
        {
            get { return level; }
            set { level = value; _changed = true; }
        }
        
        public override void Draw()
        {
            // 8 Low health - If low health can be here for 10 seconds
            // 5 med 
            // 3 High - If high can be here for 5
            // 16 - Level / 200 * 16
            if (_changed)
            {
                float v = level / 190f;
                v = v * 16;
                v = (int)v;
                ConsoleColor color = ConsoleColor.White;
                //Draw Health
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorLeft = Console.WindowWidth - 20;
                Console.CursorTop = 1;
                Console.Write("╔════════╤════╤═══╗");
                Console.CursorLeft = Console.WindowWidth - 20;
                Console.CursorTop = 2;
                Console.Write("║");
                if (v > 9 && v < 13) { color = ConsoleColor.Green; }
                else { color = ConsoleColor.DarkRed; }
                Console.ForegroundColor = color;
                for (int i = 0; i < 17; i++)
                {                    
                    if (i == 8 || i == 13)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("│");
                        Console.ForegroundColor = color;
                    }
                    else if (i < v && i != 8 && i != 13)
                    {
                        Console.Write("█");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("║");
                Console.CursorLeft = Console.WindowWidth - 20;
                Console.CursorTop = 3;
                Console.Write("╚════════╧════╧═══╝");
                _changed = true;
            }
        }
    }
}
