using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class Renderer
    {
        public void Render(int x, int y, string icon, ConsoleColor color = ConsoleColor.Black)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(icon);
            Console.ForegroundColor = prev;
        }
    }
}
